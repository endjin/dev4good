using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment.Extensions;
using Orchard.UI;
using Orchard.UI.Zones;
using Orchard.DisplayManagement;
using Szmyd.Frooth.Shapes.Behaviors;

namespace Szmyd.Frooth.Shapes
{
    
    public class LayoutShapes : IShapeTableProvider
    {
        #region Implementation of IShapeTableProvider

        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Zone")
                //Changed the type to allow nesting zones
                .OnCreating(creating => creating.BaseType = typeof(NestableZone))
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    if (shape.Current != null)
                    {
                        // Adding appropriate classes to generated zones
                        shape.Classes.Add("dyna-layout-" + ((bool)shape.Current.IsVertical ? "vertical" : "horizontal"));
                        shape.Classes.Add("dyna-" + ((bool)shape.IsLeaf ? "leaf" : "wrapper"));

                        if (shape.Current.Parent != null)
                        {
                            shape.Classes.Add("dyna-child-" + shape.Current.Position + "-" + shape.Current.Parent.Children.Count());
                        }
                        else
                        {
                            shape.Classes.Add("dyna-root");
                        }
                    }
                    else
                    {
                        shape.Classes.Add("dyna-static");
                    }
                });

            builder.Describe("Layout")
                // Changing the layout behavior so zones won't be automatically added to shape.
                // Zones will be precreated from IZoneManager instead
                .OnCreating(creating => {
                                // Hack so the default "Zones" behavior won't mess here
                                creating.Behaviors.Remove(creating.Behaviors.OfType<ZoneHoldingBehavior>().Single());
                                creating.Behaviors.Add(new FroothZonesBehavior(() => creating.New.Zone()));
                            })
                
                .OnDisplaying(displaying => {
                                  dynamic layout = displaying.Shape;
                                  // At this moment the whole magic happens 
                                  //- zones are fetched, merged, shapified and added to layout
                                  // Call to fake BuildZones() is catched by FroothZonesBehavior
                                  var built = layout.BuildZones;
                              });
        }

        #endregion

        #region Shapes
        
        /// <summary>
        /// Tweaked "Zone" shape to allow switching the wrapper tag.
        /// </summary>
        [Shape("Zone")]
        public void Zone(dynamic Display, dynamic Shape, TextWriter Output)
        {
            string id = Shape.Id;
            IEnumerable<string> classes = Shape.Classes;
            IDictionary<string, string> attributes = Shape.Attributes;
            var tag = (Shape.Current != null && Shape.Current.Tag != null) ? (string)Shape.Current.Tag : "div";
            var zoneWrapper = GetTagBuilder(tag, id, classes, attributes);
            Output.Write(zoneWrapper.ToString(TagRenderMode.StartTag));
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
            Output.Write(zoneWrapper.ToString(TagRenderMode.EndTag));
        }

        #endregion

        #region Private methods

        // Had to copy that - internal...
        static TagBuilder GetTagBuilder(string tagName, string id, IEnumerable<string> classes, IDictionary<string, string> attributes)
        {
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.MergeAttributes(attributes, false);
            foreach (var cssClass in classes ?? Enumerable.Empty<string>())
                tagBuilder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(id))
                tagBuilder.GenerateId(id);
            return tagBuilder;
        }

        #region ordered_hack

        // Had to copy the ordering logic - private...
        private static IEnumerable<object> ordered_hack(dynamic shape)
        {
            IEnumerable<dynamic> unordered = shape;
            if (unordered == null || unordered.Count() < 2)
                return shape;

            var i = 1;
            var progress = 1;
            var flatPositionComparer = new FlatPositionComparer();
            var ordering = unordered.Select(item =>
            {
                var position = (item == null || item.GetType().GetProperty("Metadata") == null || item.Metadata.GetType().GetProperty("Position") == null)
                                   ? null
                                   : item.Metadata.Position;
                return new { item, position };
            }).ToList();

            // since this isn't sticking around (hence, the "hack" in the name), throwing (in) a gnome 
            while (i < ordering.Count())
            {
                if (flatPositionComparer.Compare(ordering[i].position, ordering[i - 1].position) > -1)
                {
                    if (i == progress)
                        progress = ++i;
                    else
                        i = progress;
                }
                else
                {
                    var higherThanItShouldBe = ordering[i];
                    ordering[i] = ordering[i - 1];
                    ordering[i - 1] = higherThanItShouldBe;
                    if (i > 1)
                        --i;
                }
            }

            return ordering.Select(ordered => ordered.item).ToList();
        }

        #endregion
        #endregion
    }
}