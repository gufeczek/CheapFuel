package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.SimpleMapFuelStation
import retrofit2.Response

class FuelStationRepository {

    suspend fun getSimpleMapFuelStations(fuelTypeId: Long): Response<Array<SimpleMapFuelStation>> {
        return RetrofitInstance.fuelStationApi.getSimpleMapFuelStations(fuelTypeId)
    }

}