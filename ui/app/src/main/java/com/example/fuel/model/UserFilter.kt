package com.example.fuel.model

import com.example.fuel.enums.AccountStatus
import com.example.fuel.enums.Role

data class UserFilter(
    var role: Role?,
    var accountStatus: AccountStatus?,
    var searchPhrase: String?) {

    fun copy(): UserFilter {
        return UserFilter(
            this.role,
            this.accountStatus,
            this.searchPhrase
        )
    }

    override fun equals(other: Any?): Boolean {
        if (this === other) return true
        if (javaClass != other?.javaClass) return false

        other as UserFilter

        if (role != other.role) return false
        if (accountStatus != other.accountStatus) return false
        if (searchPhrase != other.searchPhrase) return false

        return true
    }

    override fun hashCode(): Int {
        var result = role?.hashCode() ?: 0
        result = 31 * result + (accountStatus?.hashCode() ?: 0)
        result = 31 * result + (searchPhrase?.hashCode() ?: 0)
        return result
    }
}
