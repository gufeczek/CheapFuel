package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.mock.Auth
import com.example.fuel.model.review.Review
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.model.review.NewReview
import com.example.fuel.model.review.UpdateReview
import retrofit2.Response

class ReviewRepository {

    suspend fun getFuelStationReviews(fuelStationId: Long, pageRequest: PageRequest): Response<Page<Review>> {
        return RetrofitInstance.reviewApiService.getFuelStationReviews(fuelStationId, pageRequest.toQueryMap())
    }

    suspend fun getUserReviewOfFuelStation(fuelStationId: Long): Response<Review> {
        return RetrofitInstance.reviewApiService.getUserReviewOfFuelStation(fuelStationId, Auth.username, Auth.token)
    }

    suspend fun createFuelStationReview(review: NewReview): Response<Review> {
        return RetrofitInstance.reviewApiService.postFuelStationReview(review, Auth.token)
    }

    suspend fun editFuelStationReview(reviewId: Long, review: UpdateReview): Response<Review> {
        return RetrofitInstance.reviewApiService.updateFuelStationReview(reviewId, review, Auth.token)
    }

    suspend fun deleteFuelStationReview(reviewId: Long): Response<Void> {
        return RetrofitInstance.reviewApiService.deleteFuelStationReview(reviewId, Auth.token);
    }
}