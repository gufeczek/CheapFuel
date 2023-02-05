package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.enums.AccountStatus
import com.example.fuel.enums.Role
import com.example.fuel.model.UserDetails
import com.example.fuel.model.UserFilter
import com.example.fuel.model.page.Page
import com.example.fuel.model.page.PageRequest
import com.example.fuel.repository.UserRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class UserListViewModel(
    private val userRepository: UserRepository): ViewModel() {

    private val sortProperty = "Username"
    private val sortDirection = "Asc"

    var users: MutableLiveData<Response<Page<UserDetails>>> = MutableLiveData()

    private var isUserInitialized = false

    private var _filter: UserFilter? = null
    val filter get() = _filter!!

    private var currentFilter: UserFilter? = null

    private var _roles = arrayOf(Role.USER, Role.OWNER, Role.ADMIN)
    val roles get() = _roles

    private var _accountStatuses = arrayOf(AccountStatus.NEW, AccountStatus.ACTIVE, AccountStatus.BANNED)
    val accountStatuses get() = _accountStatuses

    fun getFirstPageOfUsers() {
        viewModelScope.launch {
            initFilter()

            val pageRequest = PageRequest(1, 10, sortProperty, sortDirection)
            fetchUsersIfFilterChanged(pageRequest)
        }
    }

    private fun initFilter() {
        if (_filter == null) {
            _filter = UserFilter(null, null, null)
        }
    }

    private suspend fun fetchUsersIfFilterChanged(pageRequest: PageRequest) {
        if (filter != currentFilter) {
            currentFilter = filter.copy()
            users.value = userRepository.getAllUsers(currentFilter!!, pageRequest)
        }
    }

    fun getNextPageOfUsers() {
        viewModelScope.launch {
            val nextPage = (if (users.value != null) users.value?.body()?.nextPage else 1) ?: return@launch

            val pageRequest = PageRequest(nextPage, 10, sortProperty, sortDirection)
            users.value = userRepository.getAllUsers(filter, pageRequest)
        }
    }

    fun hasMoreUsers(): Boolean = users.value?.body()?.nextPage != null

    fun hasAnyUsers(): Boolean = users.value?.body()?.totalElements != null
            && users.value!!.body()!!.totalElements > 0

    fun isFirstPage(): Boolean = users.value?.body()?.pageNumber == 1

    fun onSearchPhraseChange(searchPhrase: String?) {
        _filter?.let {
            filter.searchPhrase = if (searchPhrase?.trim().isNullOrEmpty()) null else searchPhrase!!.trim()
        }
    }

    fun onRoleSelected(role: Role) {
        _filter?.let {
            filter.role = if (filter.role == role) null else role
        }
    }

    fun onAccountStatusSelected(status: AccountStatus) {
        _filter?.let {
            filter.accountStatus = if (filter.accountStatus == status) null else status
        }
    }

    fun isRoleSelected(role: Role): Boolean {
        return _filter?.role == role
    }

    fun isAccountStatusSelected(status: AccountStatus): Boolean {
        return _filter?.accountStatus == status
    }

    fun clear() {
        users = MutableLiveData()
        _filter = null
        currentFilter = null
    }
}