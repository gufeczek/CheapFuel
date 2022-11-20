package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.Review
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import retrofit2.Response

class ReviewRepository {

    suspend fun getFuelStationReviews(fuelStationId: Long, pageRequest: PageRequest): Response<Page<Review>> {
        return RetrofitInstance.reviewApiService.getFuelStationReviews(fuelStationId, pageRequest.toQueryMap())
    }
}