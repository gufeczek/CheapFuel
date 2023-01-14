package com.example.fuel.ui.fragment.fuelstation

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.lifecycleScope
import com.example.fuel.R
import com.example.fuel.databinding.FragmentNewFuelStationBinding
import com.example.fuel.model.FuelStationService
import com.example.fuel.model.FuelType
import com.example.fuel.model.StationChain
import com.example.fuel.model.fuelstation.Address
import com.example.fuel.model.fuelstation.GeographicalCoordinates
import com.example.fuel.model.fuelstation.NewFuelStation
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.ui.common.toggleChipAppearance
import com.example.fuel.utils.DataLoadedIndicator
import com.example.fuel.viewmodel.NewFuelStationViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch

class NewFuelStationFragment : Fragment() {
    private lateinit var viewModel: NewFuelStationViewModel
    private lateinit var binding: FragmentNewFuelStationBinding
    private lateinit var dataLoadedIndicator: DataLoadedIndicator

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[NewFuelStationViewModel::class.java]
        binding = FragmentNewFuelStationBinding.inflate(inflater, container, false)

        dataLoadedIndicator = DataLoadedIndicator(3)
        lifecycleScope.launch { collectDataLoaded() }

        initStationChainChips()
        initFuelTypesChips()
        initServicesChips()
        initCreateButton()
        initNewFuelStationObserver()

        return binding.root
    }

    private suspend fun collectDataLoaded() {
        dataLoadedIndicator.isAllDataLoaded().collectLatest {
            showLayout()
        }
    }

    private fun initNewFuelStationObserver() {
        viewModel.newFuelStation.observe(viewLifecycleOwner) { response ->

            val text = if (response.isSuccessful) resources.getString(R.string.added)
            else resources.getString(R.string.an_error_occurred)
            val toast = Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT)
            toast.show()
        }
    }

    private fun initStationChainChips() {
        viewModel.getStationChains()
        viewModel.stationChains.observe(viewLifecycleOwner) { response ->
            val stationChainChips: ChipGroup = binding.cgStationChains
            stationChainChips.removeAllViews()

            response.body()?.data?.forEach { stationChain ->
                stationChainChips.addView(createStationChainChip(stationChain))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun initFuelTypesChips() {
        viewModel.getFuelTypes()
        viewModel.fuelTypes.observe(viewLifecycleOwner) { response ->
            val fuelTypeChips: ChipGroup = binding.cgFuelTypes
            fuelTypeChips.removeAllViews()

            response.body()?.data?.forEach { fuelType ->
                fuelTypeChips.addView(createFuelTypeChip(fuelType))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun initServicesChips() {
        viewModel.getFuelStationServices()
        viewModel.services.observe(viewLifecycleOwner) { response ->
            val serviceChips: ChipGroup = binding.cgServices
            serviceChips.removeAllViews()

            response.body()?.data?.forEach { service ->
                serviceChips.addView(createServiceChip(service))
            }

            lifecycleScope.launch { dataLoadedIndicator.inc() }
        }
    }

    private fun createStationChainChip(stationChain: StationChain): Chip {
        val chip = createGenericChip()
        chip.text = stationChain.name

        chip.setOnClickListener {
            viewModel.onStationChainSelected(stationChain.id)
        }
        chip.isChecked = viewModel.isStationChainSelected(stationChain.id)
        return chip
    }

    private fun createFuelTypeChip(fuelType: FuelType): Chip {
        val chip = createGenericChip()
        chip.text = fuelType.name

        chip.setOnClickListener {
            viewModel.onFuelTypeSelected(fuelType.id)
        }
        chip.isChecked = viewModel.isFuelTypeSelected(fuelType.id)
        return chip
    }

    private fun createServiceChip(service: FuelStationService): Chip {
        val chip = createGenericChip()
        chip.text = service.name

        chip.setOnClickListener {
            viewModel.onServiceSelected(service.id)
        }
        chip.isChecked = viewModel.isServiceSelected(service.id)
        return chip
    }

    private fun createGenericChip(): Chip {
        val chip = LayoutInflater.from(requireContext()).inflate(R.layout.filter_chip, null, false) as Chip
        initChipAppearance(chip, requireContext())
        chip.setOnCheckedChangeListener { button, _ -> toggleChipAppearance(button as Chip, requireContext()) }
        return chip
    }

    private fun initCreateButton() {
        val button = binding.mbCreate

        button.setOnClickListener {
            val address = Address(
                binding.tietFuelStationCity.text.toString(),
                binding.tietFuelStationStreet.text.toString(),
                binding.tietFuelStationStreetNumber.text.toString(),
                binding.tietFuelStationPostalCode.text.toString()
            )
            val geographicalCoordinates = GeographicalCoordinates(
                binding.tietLatitudeContent.text.toString().toDouble(),
                binding.tietLongitudeContent.text.toString().toDouble()
            )
            val fuelStation = NewFuelStation(
                binding.tietFuelStationName.text.toString(),
                address,
                geographicalCoordinates,
                viewModel.getStationChainId(),
                viewModel.getFuelTypesIds(),
                viewModel.getServicesIds()
            )

            viewModel.createFuelStation(fuelStation)
        }
    }

    private fun showLayout() {
        binding.llcNewFuelStationContainer.visibility = View.VISIBLE
        binding.mbCreate.visibility = View.VISIBLE
        binding.pbFuelStationLoad.visibility = View.GONE
        binding.mbCreate.isEnabled = true
    }
}