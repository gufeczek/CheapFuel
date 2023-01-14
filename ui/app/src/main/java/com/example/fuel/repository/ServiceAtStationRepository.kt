package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.utils.Auth
import com.example.fuel.model.serviceAtStation.AddServiceToStation
import com.example.fuel.model.serviceAtStation.ServiceAtStation
import retrofit2.Response

class ServiceAtStationRepository {

    suspend fun addServiceToStation(addServiceToStation: AddServiceToStation): Response<ServiceAtStation> {
        return RetrofitInstance.serviceAtStationService.createServiceAtStation(addServiceToStation, Auth.token)
    }

    suspend fun deleteServiceFromStation(fuelStationId: Long, serviceId: Long): Response<Void> {
        return RetrofitInstance.serviceAtStationService.deleteServiceAtStation(fuelStationId, serviceId, Auth.token)
    }
}