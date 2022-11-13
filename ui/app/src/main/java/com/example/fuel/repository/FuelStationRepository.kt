package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.FuelStationsFilter
import com.example.fuel.model.SimpleMapFuelStation
import retrofit2.Response

class FuelStationRepository {

    suspend fun getSimpleMapFuelStations(filter: FuelStationsFilter): Response<Array<SimpleMapFuelStation>> {
        return RetrofitInstance.fuelStationApi.getSimpleMapFuelStations(filter)
    }

}