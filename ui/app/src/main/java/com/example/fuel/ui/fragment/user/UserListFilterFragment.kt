package com.example.fuel.ui.fragment.user

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentUserListFilterBinding
import com.example.fuel.enums.AccountStatus
import com.example.fuel.enums.Role
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.ui.common.toggleChipAppearance
import com.example.fuel.viewmodel.UserListViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup


class UserListFilterFragment : Fragment() {
    private lateinit var binding: FragmentUserListFilterBinding
    private lateinit var viewModel: UserListViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[UserListViewModel::class.java]
        binding = FragmentUserListFilterBinding.inflate(inflater, container, false)

        initFragmentWithStartingData()

        return binding.root
    }

    private fun initFragmentWithStartingData() {
        initRolesChips()
        initAccountStatusChips()
    }

    private fun initRolesChips() {
        val roleChipGroup: ChipGroup = binding.cgRoles
        roleChipGroup.removeAllViews()

        viewModel.roles.forEach { role ->
            roleChipGroup.addView(createRoleChip(role))
        }
    }

    private fun createRoleChip(role: Role): Chip {
        val chip = createGenericChip()
        chip.text = role.value
        chip.setOnClickListener { viewModel.onRoleSelected(role) }
        chip.isChecked = viewModel.isRoleSelected(role)
        return chip
    }

    private fun initAccountStatusChips() {
        val statusChipGroup: ChipGroup = binding.cgAccountStatuses
        statusChipGroup.removeAllViews()

        viewModel.accountStatuses.forEach { status ->
            statusChipGroup.addView(createStatusChip(status))
        }
    }

    private fun createStatusChip(status: AccountStatus): Chip {
        val chip = createGenericChip()
        chip.text = status.value
        chip.setOnClickListener { viewModel.onAccountStatusSelected(status) }
        chip.isChecked = viewModel.isAccountStatusSelected(status)
        return chip
    }

    private fun createGenericChip(): Chip {
        val chip = LayoutInflater.from(requireContext()).inflate(R.layout.filter_chip, null, false) as Chip
        initChipAppearance(chip, requireContext())
        chip.setOnCheckedChangeListener { button, _ -> toggleChipAppearance(button as Chip, requireContext()) }
        return chip
    }
}