package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import retrofit2.Response

class FuelStationServiceRepository {

    suspend fun getFuelStationServices(pageRequest: PageRequest): Response<Page<FuelStationService>> {
        return RetrofitInstance.fuelStationServiceApiService.getFuelStationServices(pageRequest.toQueryMap())
    }
}