using System.Data;
using Orchard.Data.Migration;

namespace oforms {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            // Creating table OFormPartRecord
            SchemaBuilder.CreateTable("OFormPartRecord", table => table
                .ContentPartRecord()
                .Column("Name", DbType.String, x => x.Unique())
                .Column("Method", DbType.String)
                .Column("InnerHtml", DbType.String)
                .Column("Action", DbType.String)
                .Column("RedirectUrl", DbType.String)
                .Column("CanUploadFiles", DbType.Boolean)
                .Column("UploadFileSizeLimit", DbType.Int64)
                .Column("UploadFileType", DbType.String)
                .Column("UseCaptcha", DbType.Boolean)
                .Column("SendEmail", DbType.Boolean)
                .Column("EmailFromName", DbType.String)
                .Column("EmailFrom", DbType.String)
                .Column("EmailSubject", DbType.String)
                .Column("EmailSendTo", DbType.String)
                .Column("EmailTemplate", DbType.String)
            );

            return 1;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("OFormPartRecord",
                table =>
                {
                    table.AddColumn("SaveResultsToDB", DbType.Boolean);
                    table.AddColumn("ValRequiredFields", DbType.String);
                    table.AddColumn("ValNumbersOnly", DbType.String);
                    table.AddColumn("ValLettersOnly", DbType.String);
                    table.AddColumn("ValLettersAndNumbersOnly", DbType.String);
                    table.AddColumn("ValDate", DbType.String);
                    table.AddColumn("ValEmail", DbType.String);
                    table.AddColumn("ValUrl", DbType.String);
                }

           );
            return 2;
        }

        public int UpdateFrom2()
        {
            SchemaBuilder.AlterTable("OFormPartRecord",
                table =>
                {
                    table.AlterColumn("InnerHtml", x => x.WithType(DbType.String).Unlimited());
                    table.AlterColumn("EmailTemplate", x => x.WithType(DbType.String).Unlimited());
                }
           );

            return 3;
        }

        public int UpdateFrom3()
        {
            SchemaBuilder.CreateTable("OFormResultRecord",
            table => table
                .Column<int>("Id", x => x.PrimaryKey().Identity())
                .Column<string>("Xml", x => x.Unlimited())
                .Column<int>("OFormPartRecord_Id")
            );

            return 4;
        }

        public int UpdateFrom4()
        {
            SchemaBuilder.AlterTable("OFormResultRecord",
            table => {
                    table.AddColumn("CreatedDate", DbType.DateTime);
                    table.AddColumn("Ip", DbType.String);
                }
            );

            return 5;
        }

        public int UpdateFrom5()
        {
            SchemaBuilder.AlterTable("OFormPartRecord",
                table =>
                {
                    table.AlterColumn("ValRequiredFields", x => x.WithType(DbType.String).WithLength(800));
                    table.AlterColumn("ValNumbersOnly", x => x.WithType(DbType.String).WithLength(800));
                    table.AlterColumn("ValLettersOnly", x => x.WithType(DbType.String).WithLength(800));
                    table.AlterColumn("ValLettersAndNumbersOnly", x => x.WithType(DbType.String).WithLength(800));
                    table.AlterColumn("ValDate", x => x.WithType(DbType.String).WithLength(800));
                    table.AlterColumn("ValEmail", x => x.WithType(DbType.String).WithLength(800));
                    table.AlterColumn("ValUrl", x => x.WithType(DbType.String).WithLength(800));
                }
            );

            return 6;
        }

        public int UpdateFrom6()
        {
            SchemaBuilder.CreateTable("OFormFileRecord",
                table =>
                {
                    table.Column<int>("Id", x => x.PrimaryKey().Identity());
                    table.Column<int>("OFormResultRecord_Id");
                    table.Column<string>("FieldName");
                    table.Column<string>("OriginalName");
                    table.Column<string>("ContentType");
                    table.Column<byte[]>("Bytes", x => x.WithType(DbType.Binary).Unlimited());
                    table.Column<long>("Size");
                }
            );

            return 7;
        }
    }
}