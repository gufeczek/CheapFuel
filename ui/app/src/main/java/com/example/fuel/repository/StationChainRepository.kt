package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.StationChain
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import retrofit2.Response

class StationChainRepository {

    suspend fun getStationChains(pageRequest: PageRequest): Response<Page<StationChain>> {
        return RetrofitInstance.stationChainApiService.getStationChains(pageRequest.toQueryMap())
    }
}