package com.example.fuel.api

import com.example.fuel.model.Post
import retrofit2.http.GET

interface SimpleApi {

    @GET("posts/1")
    suspend fun getPost(): Post
}