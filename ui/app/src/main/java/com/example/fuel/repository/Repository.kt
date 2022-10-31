package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.Post

class Repository {

    suspend fun getPost(): Post {
        return RetrofitInstance.api.getPost()
    }
}