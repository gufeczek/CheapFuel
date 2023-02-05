package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.model.report.Report
import com.example.fuel.model.report.ReportReview
import com.example.fuel.utils.Auth
import retrofit2.Response

class ReportRepository {

    suspend fun reportReview(reviewId: Long): Response<Report> {
        return RetrofitInstance.reportApiService.createReviewReport(ReportReview(reviewId), Auth.token)
    }

    suspend fun getAllReports(pageRequest: PageRequest): Response<Page<Report>> {
        return RetrofitInstance.reportApiService.getAllReports(pageRequest.toQueryMap(), Auth.token)
    }
}