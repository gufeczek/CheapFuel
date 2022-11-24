package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.mock.Auth
import com.example.fuel.model.favourite.NewFavourite
import com.example.fuel.model.favourite.UserFavourite
import retrofit2.Response

class FavouriteRepository {

    suspend fun getUserFavourite(username: String, fuelStationId: Long): Response<UserFavourite> {
        return RetrofitInstance.favouriteApiService.getFavourite(username, fuelStationId, Auth.token)
    }

    suspend fun addToFavourite(fuelStationId: Long): Response<UserFavourite> {
        return RetrofitInstance.favouriteApiService.createFavourite(NewFavourite(fuelStationId), Auth.token)
    }

    suspend fun deleteFromFavourite(fuelStationId: Long): Response<Void> {
        return RetrofitInstance.favouriteApiService.deleteFavourite(fuelStationId, Auth.token)
    }
}