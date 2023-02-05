package com.example.fuel.ui.fragment.user

import android.content.Context
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.Menu
import android.view.MenuInflater
import android.view.MenuItem
import android.view.View
import android.view.ViewGroup
import android.widget.ScrollView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.SearchView
import androidx.core.widget.NestedScrollView
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentUserListBinding
import com.example.fuel.viewmodel.UserListViewModel
import com.example.fuel.viewmodel.ViewModelFactory


class UserListFragment : Fragment(R.layout.fragment_user_list) {
    private lateinit var binding: FragmentUserListBinding
    private lateinit var viewModel: UserListViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        setHasOptionsMenu(true)

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserListViewModel::class.java]
        binding = FragmentUserListBinding.inflate(inflater, container, false)

        initUserSection()
        initUsersObserver()

        return binding.root
    }

    private fun initUserSection() {
        viewModel.getFirstPageOfUsers()

        binding.nsvUserList
            .setOnScrollChangeListener { v, _, scrollY, _, oldScrollY ->
                val nsv = v as NestedScrollView

                if (oldScrollY < scrollY
                    && scrollY == (nsv.getChildAt(0).measuredHeight - nsv.measuredHeight)
                    && viewModel.hasMoreUsers()) {

                    loadUsers()
                }
            }
    }

    private fun loadUsers() {
        viewModel.getNextPageOfUsers()
    }

    private fun initUsersObserver() {
        viewModel.users.observe(viewLifecycleOwner) { response ->
            val fragmentManager = childFragmentManager
            val fragmentTransaction = fragmentManager.beginTransaction()
            val parent = binding.llcUsersContainer

            if (viewModel.isFirstPage()) {
                scrollToTop()
                showUsersProgressBar()
                parent.removeAllViews()
            }

            if (!viewModel.hasAnyUsers()) {
                hideUsersProgressBar()
                showPlaceholder()
                return@observe
            } else {
                hidePlaceholder()
            }

            val page = response.body()

            for (user in page?.data!!) {
                val userCardFragment = UserCardFragment(user)
                fragmentTransaction.add(parent.id, userCardFragment)
            }
            fragmentTransaction.commitNow()

            if (!viewModel.hasMoreUsers()) hideUsersProgressBar()
        }
    }

    private fun scrollToTop() {
        binding.nsvUserList.fullScroll(ScrollView.FOCUS_UP)
    }

    private fun showUsersProgressBar() {
        binding.pbUsersLoad.visibility = View.VISIBLE
    }

    private fun hideUsersProgressBar() {
        binding.pbUsersLoad.visibility = View.GONE
    }

    private fun showPlaceholder() {
        binding.clPlaceholder.visibility = View.VISIBLE
    }

    private fun hidePlaceholder() {
        binding.clPlaceholder.visibility = View.GONE
    }

    override fun onCreateOptionsMenu(menu: Menu, inflater: MenuInflater) {
        inflater.inflate(R.menu.user_menu, menu)
        val searchItem = menu.findItem(R.id.user_search)
        val searchView = searchItem.actionView as SearchView

        searchView.apply {
            isIconified = false
            setOnQueryTextListener(object : SearchView.OnQueryTextListener{
                override fun onQueryTextChange(newText: String?): Boolean {
                    if (newText.isNullOrEmpty()) {
                        onSearchPhraseChangeSubmitted(newText)
                    }
                    return false
                }

                override fun onQueryTextSubmit(query: String?): Boolean {
                    onSearchPhraseChangeSubmitted(query)
                    return false
                }
            })
        }
    }

    private fun onSearchPhraseChangeSubmitted(searchPhrase: String?) {
        viewModel.onSearchPhraseChange(searchPhrase)
        viewModel.getFirstPageOfUsers()
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        if (item.itemId == R.id.user_filter) {
            Navigation.findNavController(binding.root).navigate(R.id.userListFilterFragment)
        }

        return super.onOptionsItemSelected(item)
    }

    override fun onAttach(context: Context) {
        super.onAttach(context)

        val appActivity = (activity as AppCompatActivity)
        if (!appActivity.supportActionBar?.isShowing!!) {
            appActivity.supportActionBar?.show()
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()

        viewModel.clear()
    }
}