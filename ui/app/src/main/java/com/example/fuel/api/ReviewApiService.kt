package com.example.fuel.api

import com.example.fuel.model.review.Review
import com.example.fuel.model.page.Page
import com.example.fuel.model.review.NewReview
import com.example.fuel.model.review.UpdateReview
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.Headers
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path
import retrofit2.http.QueryMap

interface ReviewApiService {

    @GET("api/v1/reviews/fuel-station/{id}")
    suspend fun getFuelStationReviews(
        @Path("id") fuelStationId: Long,
        @QueryMap pageRequest: Map<String, String>,
        @Header("Authorization") authHeader: String
    ): Response<Page<Review>>

    @GET("api/v1/reviews/user/{username}")
    suspend fun getUserReviews(
        @Path("username") username: String,
        @QueryMap pageRequest: Map<String, String>,
        @Header("Authorization") authHeader: String
    ): Response<Page<Review>>

    @GET("api/v1/reviews/{fuelStationId}/{username}")
    suspend fun getUserReviewOfFuelStation(
        @Path("fuelStationId") fuelStationId: Long,
        @Path("username") username: String,
        @Header("Authorization") authHeader: String
    ): Response<Review>

    @Headers("Content-Type:application/json")
    @POST("api/v1/reviews")
    suspend fun postFuelStationReview(
        @Body review: NewReview,
        @Header("Authorization") authHeader: String
    ): Response<Review>

    @Headers("Content-Type:application/json")
    @PUT("api/v1/reviews/{reviewId}")
    suspend fun updateFuelStationReview(
        @Path("reviewId") id: Long,
        @Body updateReview: UpdateReview,
        @Header("Authorization") authHeader: String
    ): Response<Review>

    @DELETE("api/v1/reviews/{reviewId}")
    suspend fun deleteFuelStationReview(
        @Path("reviewId") id: Long,
        @Header("Authorization") authHeader: String
    ): Response<Void>
}