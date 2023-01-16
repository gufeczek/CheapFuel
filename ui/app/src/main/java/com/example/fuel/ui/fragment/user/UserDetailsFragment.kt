package com.example.fuel.ui.fragment.user

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.ViewGroup
import android.widget.PopupMenu
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.core.widget.NestedScrollView
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentUserDetailsBinding
import com.example.fuel.enums.AccountStatus
import com.example.fuel.enums.Role
import com.example.fuel.utils.Auth
import com.example.fuel.viewmodel.UserDetailsViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.dialog.MaterialAlertDialogBuilder

class UserDetailsFragment : Fragment(R.layout.fragment_user_details) {
    private lateinit var viewModel: UserDetailsViewModel
    private lateinit var binding: FragmentUserDetailsBinding
    private var username: String? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserDetailsViewModel::class.java]
        binding = FragmentUserDetailsBinding.inflate(inflater, container, false)

        username = Auth.username

        setAppBarTitle()
        loadUserData()
        initBannedObserver()
        initReviewSection()
        initReviewObserver()
        initDeleteReviewObserver()
        initDeactivateObserver()
        initChangePasswordObserver()
        initPopupMenu()

        return binding.root
    }

    private fun loadUserData() {
        viewModel.getUser(username!!)
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

    private fun setAppBarTitle() {
        (activity as AppCompatActivity).supportActionBar?.title = username!!
    }

    private fun initBannedObserver() {
        viewModel.blockUser.observe(viewLifecycleOwner) { response ->
            val text = if (response.isSuccessful) getString(R.string.blocked)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initDeactivateObserver() {
        viewModel.deactivateUser.observe(viewLifecycleOwner) { response ->
            val text = if (response.isSuccessful) getString(R.string.deactivated)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initChangePasswordObserver() {
        viewModel.changePasswordResponse.observe(viewLifecycleOwner) { response ->
            val text = if (response.isSuccessful) getString(R.string.password_changed)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initReviewSection() {
        loadReviews()

        binding.nsvUserDetailsContainer
            .setOnScrollChangeListener { v, _, scrollY, _, oldScrollY ->
                val nsv = v as NestedScrollView

                if (oldScrollY < scrollY
                    && scrollY == (nsv.getChildAt(0).measuredHeight - nsv.measuredHeight)
                    && viewModel.hasMoreReviews()) {

                    loadReviews()
                }
            }
    }

    private fun loadReviews() {
        viewModel.getNextPageOfUserReviews(username!!)
    }

    private fun initReviewObserver() {
        viewModel.reviews.observe(viewLifecycleOwner) { response ->
            val fragmentManager = childFragmentManager
            val fragmentTransaction = fragmentManager.beginTransaction()
            val parent = binding.llcReviewsContainer

            val page = response.body()

            for (review in page?.data!!) {
                if (review.username == Auth.username) continue

                val reviewFragment = UserReviewFragment(review)
                fragmentTransaction.add(parent.id, reviewFragment)
            }
            fragmentTransaction.commitNow()

            if (!viewModel.hasMoreReviews()) hideReviewSectionProgressBar()
        }
    }

    private fun initDeleteReviewObserver() {
        viewModel.deleteReview.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                refreshReviews()
                showReviewSectionProgressBar()
            }

            val text = if (response.isSuccessful) resources.getString(R.string.deleted)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun refreshReviews() {
        val parent = binding.llcReviewsContainer
        parent.removeAllViews()

        viewModel.getFirstPageOfReviews(username!!)
    }

    private fun showReviewSectionProgressBar() {
        binding.pbReviewsLoad.visibility = View.VISIBLE
    }

    private fun hideReviewSectionProgressBar() {
        binding.pbReviewsLoad.visibility = View.GONE
    }

    private fun showLayout() {
        binding.pbUserDetailsLoad.visibility = View.GONE
        binding.nsvUserDetailsContainer.visibility = View.VISIBLE
    }

    fun initPopupMenu() {
        val actionButton = binding.acibUserActionButton

        actionButton.setOnClickListener {
            val popupMenu = PopupMenu(requireActivity(), actionButton)

            if (Auth.role == Role.ADMIN && username != Auth.username) initPopupMenuItemsForAdmin(popupMenu)
            if (username == Auth.username) initPopupMenuItemsForUser(popupMenu)

            popupMenu.show()
        }
    }

    private fun initPopupMenuItemsForAdmin(popupMenu: PopupMenu) {
        val banItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, resources.getString(R.string.ban_user))
        val deactivateItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, resources.getString(R.string.delete))

        banItem.setOnMenuItemClickListener {
            openBanView()
            true
        }

        deactivateItem.setOnMenuItemClickListener {
            askForDeleteConfirmationOfUser()
            true
        }
    }

    private fun initPopupMenuItemsForUser(popupMenu: PopupMenu) {
        val changePasswordItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 3, getString(R.string.change_password))

        changePasswordItem.setOnMenuItemClickListener {
            openChangePasswordView()
            true
        }
    }

    private fun openBanView() {
        val bundle = Bundle()
        bundle.putString("username", username)

        Navigation.findNavController(binding.root).navigate(R.id.blockUserEditorFragment, bundle)
    }

    private fun askForDeleteConfirmationOfUser() {
        MaterialAlertDialogBuilder(requireContext(), R.style.MaterialComponents_MaterialAlertDialog_RoundedCorners)
            .setMessage(getString(R.string.ask_if_deactivate_user))
            .setPositiveButton(resources.getString(R.string.yes)) { _, _ ->
                viewModel.deactivateUser(username!!)
            }
            .setNegativeButton(resources.getString(R.string.no), null)
            .show()
    }

    private fun openChangePasswordView() {
        Navigation.findNavController(binding.root).navigate(R.id.changePasswordFragment)
    }

    override fun onDestroyView() {
        super.onDestroyView()

        viewModel.clear()
    }
}