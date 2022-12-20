package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import com.example.fuel.model.account.UserLogin
import retrofit2.Response

class UserLoginViewModel {
    val response: MutableLiveData<Response<UserLogin>> = MutableLiveData()
    val user: MutableLiveData<UserLogin> = MutableLiveData()
}