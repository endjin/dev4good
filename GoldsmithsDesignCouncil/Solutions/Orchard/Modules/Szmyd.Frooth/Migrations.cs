using Orchard.Data.Migration;

namespace Szmyd.Frooth
{
    public class LayoutMigrations : DataMigrationImpl
    {
        public int Create()
        {
            // Table holding zone records
            SchemaBuilder.CreateTable("ZoneRecord",
                                      table => table
                                                   .Column<int>("Id", c => c.PrimaryKey().Identity())
                                                   .Column<int>("Parent_Id", c => c.Nullable())
                                                   .Column<string>("Name", c => c.NotNull().Unique())
                                                   .Column<string>("ParentName", c => c.Nullable())
                                                   .Column<string>("Tag", c => c.WithDefault("div"))
                                                   .Column<int>("Position", c => c.NotNull().WithDefault(1))
                                                   .Column<bool>("IsVertical", c => c.WithDefault(true))
                                                   .Column<bool>("IsCollapsible", c => c.WithDefault(true))
                                                   .Column<string>("Classes", c => c.Unlimited())
                                                   .Column<string>("Attributes", c => c.Unlimited())
                );

            // Table holding zone alternates for layers
            SchemaBuilder.CreateTable("ZoneAlternateRecord",
                                      table => table
                                                   .Column<int>("Id", c => c.PrimaryKey().Identity())
                                                   .Column<string>("ZoneName", c => c.NotNull())
                                                   .Column<string>("Tag", c => c.NotNull())
                                                   .Column<int>("LayerId", c => c.NotNull())
                                                   .Column<int>("Parent_Id", c => c.Nullable())
                                                   .Column<int>("Position", c => c.NotNull().WithDefault(1))
                                                   .Column<bool>("IsVertical", c => c.WithDefault(true))
                                                   .Column<bool>("IsCollapsible", c => c.WithDefault(true))
                                                   .Column<bool>("IsRemoved", c => c.WithDefault(false))
                                                   .Column<string>("Classes", c => c.Unlimited())
                                                   .Column<string>("Attributes", c => c.Unlimited()))
                ;

            SchemaBuilder.CreateTable("ResourceRecord",
                                      table => table
                                                   .Column<int>("Id", c => c.PrimaryKey().Identity())
                                                   .Column<int>("Dependency_Id", c => c.Nullable())
                                                   .Column<int>("Type", c => c.NotNull())
                                                   .Column<int>("Location", c => c.NotNull())
                                                   .Column<string>("Name", c => c.NotNull().Unique())
                                                   .Column<string>("Url")
                                                   .Column<string>("LocalPath"));

            SchemaBuilder.CreateTable("ResourceUsageRecord",
                                      table => table
                                                   .Column<int>("Id", c => c.PrimaryKey().Identity())
                                                   .Column<int>("LayerId", c => c.Nullable())
                                                   .Column<int>("Resource_Id", c => c.NotNull())
                                                   );


            return 1;
        }
    }
}