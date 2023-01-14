package com.example.fuel.ui.fragment.user

import android.os.Bundle
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.ViewGroup
import android.widget.PopupMenu
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentUserCardBinding
import com.example.fuel.model.UserDetails
import com.example.fuel.viewmodel.UserListViewModel
import com.example.fuel.viewmodel.ViewModelFactory


class UserCardFragment(private val user: UserDetails) : Fragment() {
    private lateinit var viewModel: UserListViewModel
    private lateinit var binding: FragmentUserCardBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserListViewModel::class.java]
        binding = FragmentUserCardBinding.inflate(inflater, container, false)

        initWithData()
        initPopupMenu()
        initOnClickListener()

        return binding.root
    }

    private fun initWithData() {
        binding.tvUsername.text = user.username
    }

    private fun initPopupMenu() {
        val actionButton = binding.acibUserActionButton

        actionButton.setOnClickListener {
            val popupMenu = PopupMenu(requireActivity(), actionButton)
            initPopupMenuWithActions(popupMenu)

            popupMenu.show()
        }
    }

    private fun initOnClickListener() {
        binding.clMainContainer.setOnClickListener {
            showUserDetails()
        }
    }

    private fun initPopupMenuWithActions(popupMenu: PopupMenu) {
        val detailsItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 0, getString(R.string.details))
        val blockItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, getString(R.string.ban_user))

        detailsItem.setOnMenuItemClickListener {
            showUserDetails()
            true
        }

        blockItem.setOnMenuItemClickListener {
            openBanView()
            true
        }
    }

    private fun showUserDetails() {
        val bundle = Bundle()
        bundle.putString("username", user.username)

        Navigation.findNavController(binding.root).navigate(R.id.userDetailsFragment, bundle)
    }

    private fun openBanView() {
        val bundle = Bundle()
        bundle.putString("username", user.username)

        Navigation.findNavController(binding.root).navigate(R.id.blockUserEditorFragment, bundle)
    }
}