package com.example.fuel.api

import com.example.fuel.model.favourite.NewFavourite
import com.example.fuel.model.favourite.UserFavourite
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Path

interface FavouriteApiService {

    @GET("api/v1/favourites/{username}/{fuelStationId}")
    suspend fun getFavourite(
        @Path("username") username: String,
        @Path("fuelStationId") fuelStationId: Long,
        @Header("Authorization") authHeader: String
    ): Response<UserFavourite>

    @POST("api/v1/favourites")
    suspend fun createFavourite(
        @Body favourite: NewFavourite,
        @Header("Authorization") authHeader: String
    ): Response<UserFavourite>

    @DELETE("api/v1/favourites/{fuelStationId}")
    suspend fun deleteFavourite(
        @Path("fuelStationId") fuelStationId: Long,
        @Header("Authorization") authHeader: String
    ): Response<Void>
}