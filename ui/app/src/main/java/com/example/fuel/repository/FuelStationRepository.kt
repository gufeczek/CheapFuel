package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.utils.Auth
import com.example.fuel.model.FuelStationDetails
import com.example.fuel.model.FuelStationFilterWithLocation
import com.example.fuel.model.FuelStationsFilter
import com.example.fuel.model.SimpleFuelStation
import com.example.fuel.model.calculator.MostEconomicalFuelStationRequest
import com.example.fuel.model.calculator.MostEconomicalFuelStationResponse
import com.example.fuel.model.fuelstation.NewFuelStation
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import retrofit2.Response

class FuelStationRepository {

    suspend fun getSimpleMapFuelStations(filter: FuelStationsFilter): Response<Array<SimpleFuelStation>> {
        return RetrofitInstance.fuelStationApi.getSimpleMapFuelStations(filter, Auth.token)
    }

    suspend fun getSimpleListFuelStations(filter: FuelStationFilterWithLocation, pageRequest: PageRequest): Response<Page<SimpleFuelStation>> {
        return RetrofitInstance.fuelStationApi.getSimpleListFuelStations(filter, pageRequest.toQueryMap(), Auth.token)
    }

    suspend fun getMostEconomicalFuelStation(data: MostEconomicalFuelStationRequest): Response<MostEconomicalFuelStationResponse> {
        return RetrofitInstance.fuelStationApi.getMostEconomicalFuelStation(data, Auth.token)
    }

    suspend fun getFuelStationDetails(fuelStationId: Long): Response<FuelStationDetails> {
        return RetrofitInstance.fuelStationApi.getFuelStationDetails(fuelStationId, Auth.token)
    }

    suspend fun createFuelStation(fuelStation: NewFuelStation): Response<FuelStationDetails> {
        return RetrofitInstance.fuelStationApi.createFuelStation(fuelStation, Auth.token)
    }

    suspend fun deleteFuelStation(fuelStationId: Long): Response<Void> {
        return RetrofitInstance.fuelStationApi.deleteFuelStation(fuelStationId, Auth.token)
    }
}