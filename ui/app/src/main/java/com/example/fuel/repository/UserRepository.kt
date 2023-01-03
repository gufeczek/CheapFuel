package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.mock.Auth
import com.example.fuel.model.Email
import com.example.fuel.model.UserDetails
import com.example.fuel.model.UserFilter
import com.example.fuel.model.account.UserLogin
import com.example.fuel.model.account.UserPasswordReset
import com.example.fuel.model.account.UserRegistration
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import retrofit2.Response

class UserRepository {

    suspend fun postLogin(user: UserLogin): Response<UserLogin> {
        return RetrofitInstance.accountApi.postLogin(user)
    }

    suspend fun postRegister(user: UserRegistration): Response<UserRegistration> {
        return RetrofitInstance.accountApi.postRegister(user)
    }

    suspend fun getAllUsers(filter: UserFilter, pageRequest: PageRequest): Response<Page<UserDetails>> {
        return RetrofitInstance.userApiService.getAllUsers(filter, pageRequest.toQueryMap(), Auth.token)
    }

    suspend fun postPasswordResetToken(email: Email): Response<Void> {
        return RetrofitInstance.accountApi.postPasswordResetToken(email)
    }

    suspend fun postResetPassword(user: UserPasswordReset): Response<Void> {
        return RetrofitInstance.accountApi.postResetPassword(user)
    }

    suspend fun getUserDetails(username: String): Response<UserDetails> {
        return RetrofitInstance.userApiService.getUserDetails(username, Auth.token)
    }
}