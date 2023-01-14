package com.example.fuel.repository

import com.example.fuel.api.RetrofitInstance
import com.example.fuel.mock.Auth
import com.example.fuel.model.block.BlockUser
import retrofit2.Response

class BlockUserRepository {

    suspend fun blockUser(blockUser: BlockUser): Response<BlockUser> {
        return RetrofitInstance.blockUserApiService.blockUser(blockUser, Auth.token)
    }
}