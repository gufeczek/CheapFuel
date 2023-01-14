package com.example.fuel.api

import com.example.fuel.model.page.Page
import com.example.fuel.model.report.Report
import com.example.fuel.model.report.ReportReview
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.QueryMap

interface ReportApiService {

    @POST("api/v1/reports")
    suspend fun createReviewReport(
        @Body report: ReportReview,
        @Header("Authorization") authHeader: String
    ): Response<Report>

    @GET
    suspend fun getAllReports(
        @QueryMap pageRequest: Map<String, String>,
        @Header("Authorization") authHeader: String
    ): Response<Page<Report>>
}