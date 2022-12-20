package com.example.fuel.api

import com.example.fuel.model.favourite.NewFavourite
import com.example.fuel.model.favourite.UserFavourite
import com.example.fuel.model.favourite.UserFavouriteDetails
import com.example.fuel.model.page.Page
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.QueryMap

interface FavouriteApiService {

    @GET("api/v1/favourites/{username}/{fuelStationId}")
    suspend fun getFavourite(
        @Path("username") username: String,
        @Path("fuelStationId") fuelStationId: Long,
        @Header("Authorization") authHeader: String
    ): Response<UserFavourite>

    @GET("api/v1/favourites")
    suspend fun getAllUserFavourites(
        @QueryMap pageRequest: Map<String, String>,
        @Header("Authorization") authHeader: String
    ): Response<Page<UserFavouriteDetails>>

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