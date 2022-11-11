package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.model.User
import com.example.fuel.viewmodel.UserViewModel
import retrofit2.Response
import retrofit2.http.Body

class UserRepository {

    suspend fun postLogin(user: User): Response<User> {
        return RetrofitInstance.accountApi.postLogin(user.username, user.password)
    }

    suspend fun postRegister(user: User): Response<User> {
        return RetrofitInstance.accountApi.postRegister(user)
    }
}