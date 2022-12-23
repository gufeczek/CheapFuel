package com.example.fuel.ui.fragment.user

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentUserDetailsBinding
import com.example.fuel.enums.AccountStatus
import com.example.fuel.viewmodel.UserDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory

class UserDetailsFragment : Fragment(R.layout.fragment_user_details) {
    private lateinit var viewModel: UserDetailsViewModel
    private lateinit var binding: FragmentUserDetailsBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserDetailsViewModel::class.java]
        binding = FragmentUserDetailsBinding.inflate(inflater, container, false)

        val username = requireArguments().getString("username")
        initUserObserver(username!!)
        initBannedObserver(username!!)

        return binding.root
    }

    private fun initUserObserver(username: String) {
        viewModel.getUser(username)
        viewModel.user.observe(viewLifecycleOwner) {
            initWithData()
            showLayout()
        }
    }

    private fun initWithData() {
        binding.tvUsername.text = viewModel.getUsername()
        binding.tvAccountCreatedAt.text = viewModel.getCreatedAt()
        binding.tvUserEmail.text = viewModel.getEmail()
        binding.tvEmailAddressConfirmed.text = if (viewModel.isEmailConfirmed()) getString(R.string.yes)
                                               else getString(R.string.no)
        binding.tvUserRole.text = viewModel.getRole().value
        setAccountStatus()
    }

    private fun setAccountStatus() {
        val status = viewModel.getAccountStatus()
        val color = when (status) {
            AccountStatus.NEW -> ContextCompat.getColor(requireContext(), R.color.light_blue)
            AccountStatus.ACTIVE -> ContextCompat.getColor(requireContext(), R.color.green)
            AccountStatus.BANNED -> ContextCompat.getColor(requireContext(), R.color.red)
        }

        binding.tvAccountStatus.text = status.value
        binding.tvAccountStatus.setTextColor(color)
    }

    private fun initBannedObserver(username: String) {
        // TODO: Implement after issue #174
    }

    private fun showLayout() {
        binding.pbUserDetailsLoad.visibility = View.GONE
        binding.nsvUserDetailsContainer.visibility = View.VISIBLE
    }
}