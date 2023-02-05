package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.utils.Auth
import com.example.fuel.model.fuelAtStation.AddFuelToStation
import com.example.fuel.model.fuelAtStation.FuelAtStation
import retrofit2.Response

class FuelAtStationRepository {

    suspend fun addFuelToStation(addFuelToStation: AddFuelToStation): Response<FuelAtStation> {
        return RetrofitInstance.fuelAtStationApiService.createFuelAtStation(addFuelToStation, Auth.token);
    }

    suspend fun deleteFuelFromStation(fuelStationId: Long, fuelTypeId: Long): Response<Void> {
        return RetrofitInstance.fuelAtStationApiService.deleteFuelAtStation(fuelStationId, fuelTypeId, Auth.token);
    }
}