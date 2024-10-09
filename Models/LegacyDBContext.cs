using System;
using System.Collections.Generic;

using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LegacyTest.Models
{
    public partial class LegacyDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        private IDbConnection DbConnection { get; }
        public LegacyDBContext()
        {
        }

        public LegacyDBContext(DbContextOptions<LegacyDBContext> options, IConfiguration configuration)
            : base(options)
        {
            this._configuration = configuration;
            DbConnection = new SqlConnection(this._configuration.GetConnectionString("LegacyConnection"));
        }



        public virtual DbSet<AdminModule> AdminModules { get; set; } = null!;
        public virtual DbSet<AdminOperation> AdminOperations { get; set; } = null!;
        public virtual DbSet<AdminPermission> AdminPermissions { get; set; } = null!;
        public virtual DbSet<AdminRole> AdminRoles { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<AnswerCompany> AnswerCompanies { get; set; } = null!;
        public virtual DbSet<AnswerCriterionCompany> AnswerCriterionCompanies { get; set; } = null!;
        public virtual DbSet<AnswerPerson> AnswerPeople { get; set; } = null!;
        public virtual DbSet<CharacterizationByCompany> CharacterizationByCompanies { get; set; } = null!;
        public virtual DbSet<CharacterizationEffect> CharacterizationEffects { get; set; } = null!;
        public virtual DbSet<CharacterizationRecomendation> CharacterizationRecomendations { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Criterion> Criteria { get; set; } = null!;
        public virtual DbSet<CriterionCharacterization> CriterionCharacterizations { get; set; } = null!;
        public virtual DbSet<CriterionClasification> CriterionClasifications { get; set; } = null!;
        public virtual DbSet<Dimension> Dimensions { get; set; } = null!;
        public virtual DbSet<Form> Forms { get; set; } = null!;
        public virtual DbSet<FormPlan> FormPlans { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Plan> Plans { get; set; } = null!;
        public virtual DbSet<PlanCompany> PlanCompanies { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<ReportScale> ReportScales { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<TransactionCompany> TransactionCompanies { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<ViewCriterionClasification> ViewCriterionClasifications { get; set; } = null!;
        public virtual DbSet<ViewCriterionQuestion> ViewCriterionQuestions { get; set; } = null!;
        public virtual DbSet<ViewFormClasification> ViewFormClasifications { get; set; } = null!;
        public virtual DbSet<ViewFormCriterion> ViewFormCriteria { get; set; } = null!;
        public virtual DbSet<ViewQuestion> ViewQuestions { get; set; } = null!;
        public virtual DbSet<ViewQuestionAnswer> ViewQuestionAnswers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(DbConnection.ToString(), options =>
                options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));
            }        

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminModule>(entity =>
            {
                entity.ToTable("AdminModule");

                entity.Property(e => e.Module)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AdminOperation>(entity =>
            {
                entity.ToTable("AdminOperation");

                entity.Property(e => e.Operation).IsUnicode(false);

                entity.HasOne(d => d.IdModuleNavigation)
                    .WithMany(p => p.AdminOperations)
                    .HasForeignKey(d => d.IdModule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminOperation_AdminModule");
            });

            modelBuilder.Entity<AdminPermission>(entity =>
            {
                entity.ToTable("AdminPermission");

                entity.HasOne(d => d.IdOperationNavigation)
                    .WithMany(p => p.AdminPermissions)
                    .HasForeignKey(d => d.IdOperation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminPermission_AdminOperation");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.AdminPermissions)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminPermission_AdminRole");
            });

            modelBuilder.Entity<AdminRole>(entity =>
            {
                entity.ToTable("AdminRole");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.AnswerText).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Value).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.IdQuestion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<AnswerCompany>(entity =>
            {
                entity.ToTable("AnswerCompany");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.AnswerCompanies)
                    .HasForeignKey(d => d.IdQuestion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnswerCompany_Question");
            });

            modelBuilder.Entity<AnswerCriterionCompany>(entity =>
            {
                entity.ToTable("AnswerCriterionCompany");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.IdCriterionNavigation)
                    .WithMany(p => p.AnswerCriterionCompanies)
                    .HasForeignKey(d => d.IdCriterion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnswerCriterionCompany_Criterion");

                entity.HasOne(d => d.IdPlanCompanyNavigation)
                    .WithMany(p => p.AnswerCriterionCompanies)
                    .HasForeignKey(d => d.IdPlanCompany)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnswerCriterionCompany_PlanCompany");
            });

            modelBuilder.Entity<AnswerPerson>(entity =>
            {
                entity.ToTable("AnswerPerson");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.AnswerPeople)
                    .HasForeignKey(d => d.IdQuestion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnswerPerson_Question");
            });

            modelBuilder.Entity<CharacterizationByCompany>(entity =>
            {
                entity.ToTable("CharacterizationByCompany");

                entity.Property(e => e.Characterization).IsUnicode(false);

                entity.Property(e => e.CriterionText1).IsUnicode(false);

                entity.Property(e => e.CriterionText2).IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.IdClasification1).IsUnicode(false);

                entity.Property(e => e.IdClasification2).IsUnicode(false);

                entity.Property(e => e.NameForm).IsUnicode(false);

                entity.HasOne(d => d.IdCharacterizationNavigation)
                    .WithMany(p => p.CharacterizationByCompanies)
                    .HasForeignKey(d => d.IdCharacterization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CharacterizationByCompany_CriterionCharacterization");
            });

            modelBuilder.Entity<CharacterizationEffect>(entity =>
            {
                entity.ToTable("CharacterizationEffect");

                entity.Property(e => e.Effect).IsUnicode(false);

                entity.Property(e => e.EffectType).IsUnicode(false);


                entity.HasOne(d => d.IdCharacterizationNavigation)
                    .WithMany(p => p.CharacterizationEffects)
                    .HasForeignKey(d => d.IdCharacterization)
                    .HasConstraintName("FK_CharacterizationEffect_CriterionCharacterization");
            });

            modelBuilder.Entity<CharacterizationRecomendation>(entity =>
            {
                entity.ToTable("CharacterizationRecomendation");

                entity.Property(e => e.Recomendation).IsUnicode(false);

                entity.Property(e => e.RecomendationType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCharacterizationNavigation)
                    .WithMany(p => p.CharacterizationRecomendations)
                    .HasForeignKey(d => d.IdCharacterization)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CharacterizationRecomendation_CriterionCharacterization");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.BusinessName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CommercialReg)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeReg)
                  .HasMaxLength(20)
                  .IsUnicode(false);
            });

            modelBuilder.Entity<Criterion>(entity =>
            {
                entity.ToTable("Criterion");

                entity.Property(e => e.CriterionText).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.HasOne(d => d.IdFormNavigation)
                    .WithMany(p => p.Criteria)
                    .HasForeignKey(d => d.IdForm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Criterion_Form");
            });

            modelBuilder.Entity<CriterionCharacterization>(entity =>
            {
                entity.ToTable("CriterionCharacterization");

                entity.Property(e => e.Characterization).IsUnicode(false);

                entity.Property(e => e.Clasification1).IsUnicode(false);

                entity.Property(e => e.Clasification2).IsUnicode(false);

                entity.HasOne(d => d.IdCriterion1Navigation)
                    .WithMany(p => p.CriterionCharacterizationIdCriterion1Navigations)
                    .HasForeignKey(d => d.IdCriterion1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CriterionCharacterization_Criterion");

                entity.HasOne(d => d.IdCriterion2Navigation)
                    .WithMany(p => p.CriterionCharacterizationIdCriterion2Navigations)
                    .HasForeignKey(d => d.IdCriterion2)
                    .HasConstraintName("FK_CriterionCharacterization_Criterion1");
            });

            modelBuilder.Entity<CriterionClasification>(entity =>
            {
                entity.ToTable("CriterionClasification");

                entity.Property(e => e.Clasification)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCriterionNavigation)
                    .WithMany(p => p.CriterionClasifications)
                    .HasForeignKey(d => d.IdCriterion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CriterionClasification_Criterion");
            });

            modelBuilder.Entity<Dimension>(entity =>
            {
                entity.ToTable("Dimension");

                entity.Property(e => e.DimensionText).HasMaxLength(200);
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable("Form");
            });

            modelBuilder.Entity<FormPlan>(entity =>
            {
                entity.ToTable("FormPlan");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdFormNavigation)
                    .WithMany(p => p.FormPlans)
                    .HasForeignKey(d => d.IdForm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormPlan_Form");

                entity.HasOne(d => d.IdPlanNavigation)
                    .WithMany(p => p.FormPlans)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormPlan_Plan");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdRole).HasDefaultValueSql("((2))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pswd).IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Activo')");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.IdCompanyNavigation)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.IdCompany)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_Company");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person_AdminRole");
            });

            modelBuilder.Entity<Plan>(entity =>
            {
                entity.ToTable("Plan");

                entity.Property(e => e.Bonus)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Forms)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.NamePlan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserAccount)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlanCompany>(entity =>
            {
                entity.ToTable("PlanCompany");

                entity.Property(e => e.DateEnd).HasColumnType("date");

                entity.Property(e => e.DateInitial).HasColumnType("date");

                entity.HasOne(d => d.IdCompanyNavigation)
                    .WithMany(p => p.PlanCompanies)
                    .HasForeignKey(d => d.IdCompany)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanCompany_Company");

                entity.HasOne(d => d.IdPlanNavigation)
                    .WithMany(p => p.PlanCompanies)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanCompany_Plan");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionText).IsUnicode(false);

                entity.HasOne(d => d.IdCriterionNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdCriterion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Criterion");

                entity.HasOne(d => d.IdDimensionNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdDimension)
                    .HasConstraintName("FK_Question_Dimension");

                entity.HasOne(d => d.QuestionParentNavigation)
                    .WithMany(p => p.InverseQuestionParentNavigation)
                    .HasForeignKey(d => d.QuestionParent)
                    .HasConstraintName("FK_Question_Question");
            });

            modelBuilder.Entity<ReportScale>(entity =>
            {
                entity.ToTable("ReportScale");

                entity.Property(e => e.Category)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdFormNavigation)
                    .WithMany(p => p.ReportScales)
                    .HasForeignKey(d => d.IdForm)
                    .HasConstraintName("FK_ReportScale_Form");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Session");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Stated)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionCompany>(entity =>
            {
                entity.ToTable("TransactionCompany");

                entity.Property(e => e.CodeNameBank)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CodeService)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodeTraceability)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateTransaction).HasColumnType("datetime");

                entity.Property(e => e.NumberReference)
                    .IsUnicode(false)
                    .HasColumnName("numberReference");

                entity.Property(e => e.PaymentForm)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentPlataform)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StateTransaction)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Pswd)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Activo')");

                entity.Property(e => e.UserName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewCriterionClasification>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewCriterionClasification");

                entity.Property(e => e.Characterization).IsUnicode(false);

                entity.Property(e => e.Clasification1).IsUnicode(false);

                entity.Property(e => e.Clasification2).IsUnicode(false);

                entity.Property(e => e.CriterionText1).IsUnicode(false);

                entity.Property(e => e.CriterionText2).IsUnicode(false);
            });

            modelBuilder.Entity<ViewCriterionQuestion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewCriterionQuestion");

                entity.Property(e => e.CriterionText).IsUnicode(false);

                entity.Property(e => e.NameForm).IsUnicode(false);

                entity.Property(e => e.QuestionText).IsUnicode(false);
            });

            modelBuilder.Entity<ViewFormClasification>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewFormClasification");

                entity.Property(e => e.Clasification)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CriterionText).IsUnicode(false);

                entity.Property(e => e.NameForm).IsUnicode(false);
            });

            modelBuilder.Entity<ViewFormCriterion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewFormCriterion");

                entity.Property(e => e.CriterionText).IsUnicode(false);

                entity.Property(e => e.NameForm).IsUnicode(false);
            });

            modelBuilder.Entity<ViewQuestion>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewQuestion");

                entity.Property(e => e.NameForm).IsUnicode(false);

                entity.Property(e => e.QuestionText).IsUnicode(false);
            });

            modelBuilder.Entity<ViewQuestionAnswer>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewQuestionAnswer");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
