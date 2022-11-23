using Application.Models;
using Application.Reviews.Commands.UpdateReview;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandValidatorTest
{
    private readonly UpdateReviewCommandValidator _validator;

    public UpdateReviewCommandValidatorTest()
    {
        _validator = new UpdateReviewCommandValidator();
    }
    
    [Theory]
    [InlineData(1, 1, "c")]
    [InlineData(999, 1,"content")]
    [InlineData(1, 5, null)]
    public void Validation_passes_for_correct_data(int reviewId, int rate, string content)
    {
        // Arrange
        var command = new UpdateReviewCommand(reviewId, new UpdateReviewDto{ Rate = rate, Content = content });
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewId);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Rate);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Content);
    }

    [Fact]
    public void Validation_fails_for_null_review_id()
    {
        // Arrange
        var command = new UpdateReviewCommand(null, new UpdateReviewDto{ Rate = 1, Content = "content" });
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ReviewId);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Rate);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Content);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void Validation_fails_for_review_id_smaller_than_one(int id)
    {
        // Arrange
        var command = new UpdateReviewCommand(id, new UpdateReviewDto{ Rate = 1, Content = "content" });
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ReviewId);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Rate);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Content);
    }

    [Fact]
    public void Validation_fails_for_null_review()
    {
        // Arrange
        var command = new UpdateReviewCommand(1, null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewId);
        result.ShouldHaveValidationErrorFor(c => c.ReviewDto);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Rate);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Content);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    [InlineData(6)]
    [InlineData(999)]
    public void Validation_fails_for_rate_not_between_one_and_five(int rate)
    {
        // Arrange
        var command = new UpdateReviewCommand(1, new UpdateReviewDto{ Rate = rate, Content = "content" });
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewId);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto);
        result.ShouldHaveValidationErrorFor(c => c.ReviewDto!.Rate);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Content);
    }

    [Fact]
    public void Validation_fails_for_null_rate()
    {
        // Arrange
        var command = new UpdateReviewCommand(1, new UpdateReviewDto{ Rate = null, Content = "content" });
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewId);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto);
        result.ShouldHaveValidationErrorFor(c => c.ReviewDto!.Rate);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Content);
    }

    [Fact]
    public void Validation_fails_for_content_longer_than_1000_characters()
    {
        // Arrange
        const string content = "p2A%4P4J,REh)py63nn{AUA@8Bci+#aez:_zGUt:h6jD%F{E/HFeV}76%qfA*w;B;mpRt8h8FZ" +
                               "{}zg(qxaPpf+6.h{LH!N36GgzM.SBqar(wqQ6UK#yFBzA#{[F_Z7;WC.WP,Z?52bRF6./e=HDg" +
                               "eC_EfE(Qid!!]4K.+U7{K_8M4#pvb]r4SebSWStSZy:iui3b;S_@/pf)]&qr$iVyk2_/U$:.Am" +
                               "=a=gbJ-PYKd.P?hE.8]7HH@BB=%b7ZFVHpX/!%qjVe@vK,?4F(NZ.kB?)Tbt{(VLgVReJfgd:7*" +
                               "v_GHK/vKx+L{weyJC&gt}_;;C7];Zk3XpWf=Jg[9J{;SyCE$XxkDvf%#,G*?/U*&q]z]7Vi-dL%+" +
                               "Mj$6n!AeqBPEzv%cc&k%eRmEevPjWZ#SGNc:3F%nCRu.HVA[[pRc}yQp4)7#g8%@Sf#]Y4&G7tqJ" +
                               "QYRk&D?REdQy4)S7[M?3cEt*yNG8/A+]Te?LN]5*DwG}n{=Emrtt6cM#*e4MBmNJ$6{E}7#-ck#m" +
                               "Qg]+8V;hAY[:xS2]33BwhdHB_Hi6w7Gj:,$/Bcfw4ue)USh8kW]P,iF&ag%:{e{vTa4{9@pAh6m," +
                               "4{fMrhnVSme{C5BurkdVP=E!-T5KG5#QLyydJ73=!M!$!C_,y=&d[{J7ui5}X+K;DN+++bLN%h{*" +
                               "Gtb$Kx2_:2-H3]N4j,,tV$(g+hK?GNb@PWWF;4mfAMrNSK+*F%Z7F.Xq;vy,RiF9e-jS&FPh@UjN" +
                               "!9e__AiZC[UiT:x3DB%.NLp4hyH&#uwJrm;8mF;Wb[8],zLh7!.A*!nXY2/g4FkF_vu$.b!&9/RJ" +
                               "PDLWCk4_4fGFgSH:=Hyx8CNB4/PuCB?t;*;S=iGLzRdN$P;b*,PSJzdZ][U/R*=ef)uiN$mwBnhF" +
                               "D[-;kf)p,2@V{+H[HrFuBPFgv#V%6GMXq@xae3;Qxg5)=F,/:9D)RMMkF?qMn{K/p2J},3uLpie_" +
                               "%[@Hnnb)[wK)@SJ4zX,F";
        
        var command = new UpdateReviewCommand(1, new UpdateReviewDto{ Rate = 1, Content = content });
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewId);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto);
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewDto!.Rate);
        result.ShouldHaveValidationErrorFor(c => c.ReviewDto!.Content);
    }
}