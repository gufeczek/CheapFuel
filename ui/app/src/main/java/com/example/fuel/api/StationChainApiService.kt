package com.example.fuel.api

import com.example.fuel.model.StationChain
import com.example.fuel.model.page.Page
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.QueryMap

interface StationChainApiService {

    @GET("api/v1/station-chains")
    suspend fun getStationChains(
        @QueryMap pageRequest: Map<String, String>
    ): Response<Page<StationChain>>
}