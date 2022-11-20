package com.example.fuel.api

import com.example.fuel.model.Review
import com.example.fuel.model.page.Page
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.QueryMap

interface ReviewApiService {

    @GET("api/v1/reviews/fuel-station/{id}")
    suspend fun getFuelStationReviews(
        @Path("id") fuelStationId: Long,
        @QueryMap pageRequest: Map<String, String>
    ): Response<Page<Review>>
}