package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.lifecycleScope
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationEditorBinding
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.ui.common.toggleChipAppearance
import com.example.fuel.ui.fragment.common.FullHeightBottomSheetDialogFragment
import com.example.fuel.utils.DataLoadedIndicator
import com.example.fuel.viewmodel.FuelStationDetailsViewModel
import com.example.fuel.viewmodel.FuelStationEditorViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch


class FuelStationEditorFragment : FullHeightBottomSheetDialogFragment() {
    private lateinit var binding: FragmentFuelStationEditorBinding
    private lateinit var fuelStationDetailsViewModel: FuelStationDetailsViewModel
    private lateinit var fuelStationEditorViewModel: FuelStationEditorViewModel
    private lateinit var dataLoadedIndicator: DataLoadedIndicator
    private var fuelStationId: Long? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        fuelStationDetailsViewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationDetailsViewModel::class.java]
        fuelStationEditorViewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationEditorViewModel::class.java]
        binding = FragmentFuelStationEditorBinding.inflate(inflater, container, false)
        dataLoadedIndicator = DataLoadedIndicator(2)
        lifecycleScope.launch { collectDataLoaded() }

        fuelStationEditorViewModel.setFuelStation(fuelStationDetailsViewModel.getFuelStation())
        fuelStationId = fuelStationDetailsViewModel.getFuelStationId()

        initFuelTypeChips()
        initFuelStationServiceChips()

        initAddTypeObserver()
        initRemoveTypeObserver()
        initAddServiceObserver()
        initRemoveServiceObserver()

        return binding.root
    }

    private suspend fun collectDataLoaded() {
        dataLoadedIndicator.isAllDataLoaded().collectLatest {
            binding.loadingSpinner.visibility = View.GONE
            binding.svEditorContainer.visibility = View.VISIBLE
        }
    }

    private fun initFuelTypeChips() {
        fuelStationEditorViewModel.getFuelTypes()
        fuelStationEditorViewModel.fuelTypes.observe(viewLifecycleOwner) { response ->
            val fuelTypeChipGroup: ChipGroup = binding.cgEditorFuelTypes
            fuelTypeChipGroup.removeAllViews()

            response.body()?.data?.forEach { fuelType ->
                fuelTypeChipGroup.addView(createFuelTypeChip(fuelType))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun createFuelTypeChip(fuelType: FuelType): Chip {
        val chip = createGenericChip()
        chip.text = fuelType.name
        chip.isChecked = fuelStationEditorViewModel.isFuelTypeSelected(fuelType.id)

        chip.setOnClickListener {
            fuelStationEditorViewModel.toggleFuelType(fuelType.id, chip.isChecked)
        }

        return chip
    }

    private fun initFuelStationServiceChips() {
        fuelStationEditorViewModel.getFuelStationServices()
        fuelStationEditorViewModel.fuelStationServices.observe(viewLifecycleOwner) { response ->
            val fuelStationServiceChipGroup: ChipGroup = binding.cgEditorFuelStationServices
            fuelStationServiceChipGroup.removeAllViews()

            response.body()?.data?.forEach { fuelStationService ->
                fuelStationServiceChipGroup.addView(createFuelStationServiceChip(fuelStationService))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun createFuelStationServiceChip(fuelStationService: FuelStationService): Chip {
        val chip = createGenericChip()
        chip.text = fuelStationService.name
        chip.isChecked = fuelStationEditorViewModel.isFuelStationServiceSelected(fuelStationService.id)

        chip.setOnClickListener {
            fuelStationEditorViewModel.toggleService(fuelStationService.id, chip.isChecked)
        }

        return chip
    }

    private fun createGenericChip(): Chip {
        val chip = LayoutInflater.from(requireContext()).inflate(R.layout.filter_chip, null, false) as Chip
        initChipAppearance(chip, requireContext())
        chip.setOnCheckedChangeListener { button, _ -> toggleChipAppearance(button as Chip, requireContext()) }
        return chip
    }

    private fun initAddTypeObserver() {
        fuelStationEditorViewModel.addFuelType.observe(viewLifecycleOwner) { response ->
            showAddedToast(response.isSuccessful)
        }
    }

    private fun initRemoveTypeObserver() {
        fuelStationEditorViewModel.removeFuelType.observe(viewLifecycleOwner) { response ->
            showDeletedToast(response.isSuccessful)
        }
    }

    private fun initAddServiceObserver() {
        fuelStationEditorViewModel.addService.observe(viewLifecycleOwner) { response ->
            showAddedToast(response.isSuccessful)
        }
    }

    private fun initRemoveServiceObserver() {
        fuelStationEditorViewModel.removeService.observe(viewLifecycleOwner) { response ->
            showDeletedToast(response.isSuccessful)
        }
    }

    private fun showAddedToast(isSuccessful: Boolean) {
        val text = if (isSuccessful) resources.getString(R.string.added)
                   else resources.getString(R.string.an_error_occurred)

        val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
        toast.show()
    }

    private fun showDeletedToast(isSuccessful: Boolean) {
        val text = if (isSuccessful) resources.getString(R.string.deleted)
                   else resources.getString(R.string.an_error_occurred)

        val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
        toast.show()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        fuelStationEditorViewModel.clear()

        if (fuelStationEditorViewModel.isChange()) {
            fuelStationDetailsViewModel.getFuelStationDetails(fuelStationId!!)
        }
    }

    companion object {
        const val TAG = "FuelStationEditor"
    }
}