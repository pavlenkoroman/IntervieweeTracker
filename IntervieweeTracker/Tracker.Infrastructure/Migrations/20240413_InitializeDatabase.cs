using FluentMigrator;

namespace Tracker.Infrastructure.Migrations;

[Migration(20240413)]
public class InitializeDatabase : Migration
{
    public override void Up()
    {
        Create.Table("roles")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("text").AsString();

        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString()
            .WithColumn("email").AsString()
            .WithColumn("password_hash").AsString()
            .WithColumn("role_id").AsGuid().ForeignKey("roles", "id");

        Create.Table("workflow_templates")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString();

        Create.Table("users_roles")
            .WithColumn("role_id").AsGuid().ForeignKey("roles", "id")
            .WithColumn("user_id").AsGuid().ForeignKey("users", "id");

        Create.Table("step_templates")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString()
            .WithColumn("order").AsInt32()
            .WithColumn("role_id").AsGuid().ForeignKey("roles", "id")
            .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
            .WithColumn("workflow_template_id").AsGuid().ForeignKey("workflow_templates", "id");

        Create.Table("workflows")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString()
            .WithColumn("workflow_template_id").AsGuid().ForeignKey("workflow_templates", "id");

        Create.Table("steps")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString()
            .WithColumn("order").AsInt32()
            .WithColumn("comment").AsString()
            .WithColumn("status").AsInt32()
            .WithColumn("planning_date").AsDateTime()
            .WithColumn("role_id").AsGuid().ForeignKey("roles", "id")
            .WithColumn("user_id").AsGuid().ForeignKey("users", "id")
            .WithColumn("workflow_id").AsGuid().ForeignKey("workflows", "id");

        Create.Table("requests")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("candidate_name").AsString()
            .WithColumn("candidate_resume").AsString()
            .WithColumn("candidate_email").AsString()
            .WithColumn("workflow_id").AsGuid().ForeignKey("workflows", "id");
    }

    public override void Down()
    {
        Delete.Table("roles");
        Delete.Table("users");
        Delete.Table("workflow_templates");
        Delete.Table("users_roles");
        Delete.Table("step_templates");
        Delete.Table("workflows");
        Delete.Table("steps");
        Delete.Table("requests");
    }
}