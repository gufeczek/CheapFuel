package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.mock.Auth
import com.example.fuel.model.User
import com.example.fuel.model.UserDetails
import com.example.fuel.model.UserFilter
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import retrofit2.Response

class UserRepository {

    suspend fun postLogin(user: User): Response<User> {
        return RetrofitInstance.accountApi.postLogin(user.username, user.password)
    }

    suspend fun postRegister(user: User): Response<User> {
        return RetrofitInstance.accountApi.postRegister(user)
    }

    suspend fun getAllUsers(filter: UserFilter, pageRequest: PageRequest): Response<Page<UserDetails>> {
        return RetrofitInstance.userApiService.getAllUsers(filter, pageRequest.toQueryMap(), Auth.token)
    }

    suspend fun getUserDetails(username: String): Response<UserDetails> {
        return RetrofitInstance.userApiService.getUserDetails(username, Auth.token)
    }
}