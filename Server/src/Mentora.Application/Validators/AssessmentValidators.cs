using Mentora.Application.DTOs.Assessment;
using FluentValidation;

namespace Mentora.Application.Validators
{
    /// <summary>
    /// Validator for assessment response submission per SRS 2.3.8.1
    /// </summary>
    public class SubmitAssessmentResponseValidator : AbstractValidator<SubmitAssessmentResponseDto>
    {
        public SubmitAssessmentResponseValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("Question ID is required");

            When(x => !x.IsSkipped, () =>
            {
                RuleFor(x => x.ResponseValue)
                    .NotEmpty()
                    .WithMessage("Response value is required when not skipped")
                    .MaximumLength(2000)
                    .WithMessage("Response value cannot exceed 2000 characters");
            });

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters");
        }
    }

    /// <summary>
    /// Validator for bulk assessment submission
    /// </summary>
    public class BulkAssessmentSubmissionValidator : AbstractValidator<BulkAssessmentSubmissionDto>
    {
        public BulkAssessmentSubmissionValidator()
        {
            RuleFor(x => x.AssessmentAttemptId)
                .NotEmpty()
                .WithMessage("Assessment attempt ID is required");

            RuleFor(x => x.Responses)
                .NotNull()
                .WithMessage("Responses are required")
                .NotEmpty()
                .WithMessage("At least one response is required")
                .Must(responses => responses.Count <= 100)
                .WithMessage("Cannot submit more than 100 responses at once");

            RuleForEach(x => x.Responses)
                .SetValidator(new SubmitAssessmentResponseValidator());
        }
    }

    /// <summary>
    /// Validator for study plan generation request per SRS 3.2.1
    /// </summary>
    public class GenerateStudyPlanRequestValidator : AbstractValidator<GenerateStudyPlanRequestDto>
    {
        public GenerateStudyPlanRequestValidator()
        {
            RuleFor(x => x.AssessmentAttemptId)
                .NotEmpty()
                .WithMessage("Assessment attempt ID is required");

            RuleFor(x => x.PreferredCompletionDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.PreferredCompletionDate.HasValue)
                .WithMessage("Preferred completion date must be in the future");

            RuleFor(x => x.WeeklyHoursAvailable)
                .InclusiveBetween(1, 168)
                .When(x => x.WeeklyHoursAvailable.HasValue)
                .WithMessage("Weekly hours available must be between 1 and 168");

            RuleFor(x => x.FocusAreas)
                .Must(areas => areas == null || areas.Count <= 10)
                .WithMessage("Cannot have more than 10 focus areas");

            RuleFor(x => x.AdditionalInstructions)
                .MaximumLength(1000)
                .WithMessage("Additional instructions cannot exceed 1000 characters");
        }
    }

    /// <summary>
    /// Validator for study plan progress updates per SRS 5.2
    /// </summary>
    public class UpdateStudyPlanProgressValidator : AbstractValidator<UpdateStudyPlanProgressDto>
    {
        public UpdateStudyPlanProgressValidator()
        {
            RuleFor(x => x.StudyPlanId)
                .NotEmpty()
                .WithMessage("Study plan ID is required");

            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100)
                .When(x => x.ProgressPercentage.HasValue)
                .WithMessage("Progress percentage must be between 0 and 100");

            RuleFor(x => x)
                .Must(HaveAtLeastOneUpdate)
                .WithMessage("At least one update field (StepId, CheckpointId, NewStatus, IsCompleted, ProgressPercentage) must be provided");
        }

        private bool HaveAtLeastOneUpdate(UpdateStudyPlanProgressDto dto)
        {
            return dto.StepId.HasValue ||
                   dto.CheckpointId.HasValue ||
                   !string.IsNullOrWhiteSpace(dto.NewStatus) ||
                   dto.IsCompleted.HasValue ||
                   dto.ProgressPercentage.HasValue;
        }
    }

    /// <summary>
    /// Validator for assessment question creation (admin only)
    /// </summary>
    public class AssessmentQuestionValidator : AbstractValidator<AssessmentQuestionDto>
    {
        public AssessmentQuestionValidator()
        {
            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .WithMessage("Question text is required")
                .MaximumLength(500)
                .WithMessage("Question text cannot exceed 500 characters");

            RuleFor(x => x.QuestionType)
                .NotEmpty()
                .WithMessage("Question type is required")
                .Must(type => new[] { "MultipleChoice", "Scale", "Text", "DateRange", "Boolean" }.Contains(type))
                .WithMessage("Invalid question type");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Category is required")
                .MaximumLength(100)
                .WithMessage("Category cannot exceed 100 characters");

            RuleFor(x => x.OrderIndex)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Order index must be non-negative");

            When(x => x.QuestionType == "MultipleChoice", () =>
            {
                RuleFor(x => x.Options)
                    .NotNull()
                    .WithMessage("Options are required for multiple choice questions")
                    .Must(options => options != null && options.Count >= 2)
                    .WithMessage("Multiple choice questions must have at least 2 options");
            });

            When(x => x.QuestionType == "Scale", () =>
            {
                RuleFor(x => x.MinValue)
                    .NotNull()
                    .WithMessage("Minimum value is required for scale questions");

                RuleFor(x => x.MaxValue)
                    .NotNull()
                    .WithMessage("Maximum value is required for scale questions")
                    .GreaterThan(x => x.MinValue ?? 0)
                    .WithMessage("Maximum value must be greater than minimum value");
            });

            RuleFor(x => x.HelpText)
                .MaximumLength(1000)
                .WithMessage("Help text cannot exceed 1000 characters");

            RuleFor(x => x.ValidationPattern)
                .MaximumLength(200)
                .WithMessage("Validation pattern cannot exceed 200 characters");
        }
    }
}
