package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.mock.Auth
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationPageRequest
import com.example.fuel.model.FuelStationsFilter
import com.example.fuel.model.SimpleFuelStation
import com.example.fuel.model.page.Page
import retrofit2.Response

class FuelStationRepository {

    suspend fun getSimpleMapFuelStations(filter: FuelStationsFilter): Response<Array<SimpleFuelStation>> {
        return RetrofitInstance.fuelStationApi.getSimpleMapFuelStations(filter, Auth.token)
    }

    suspend fun getSimpleListFuelStations(request: FuelStationPageRequest): Response<Page<SimpleFuelStation>> {
        return RetrofitInstance.fuelStationApi.getSimpleListFuelStations(request, Auth.token)
    }

    suspend fun getFuelStationDetails(fuelStationId: Long): Response<FuelStationDetails> {
        return RetrofitInstance.fuelStationApi.getFuelStationDetails(fuelStationId, Auth.token)
    }

    suspend fun deleteFuelStation(fuelStationId: Long): Response<Void> {
        return RetrofitInstance.fuelStationApi.deleteFuelStation(fuelStationId, Auth.token)
    }
}