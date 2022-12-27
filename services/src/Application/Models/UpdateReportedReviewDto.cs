using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models;

public class UpdateReportedReviewDto
{
    public ReportStatus ReportStatus { get; set; }
}

