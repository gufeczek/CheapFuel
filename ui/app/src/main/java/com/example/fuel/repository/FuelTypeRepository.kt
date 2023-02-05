package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.FuelType
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import retrofit2.Response

class FuelTypeRepository {

    suspend fun getFuelTypes(pageRequest: PageRequest): Response<Page<FuelType>> {
        return RetrofitInstance.fuelTypeApiService.getFuelTypes(pageRequest.toQueryMap())
    }
}