package com.example.fuel.ui.fragment.calculator

import android.app.AlertDialog
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.provider.Settings
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.widget.addTextChangedListener
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationCalculatorBinding
import com.example.fuel.model.FuelType
import com.example.fuel.ui.common.initChipAppearance
import com.example.fuel.ui.common.toggleChipAppearance
import com.example.fuel.ui.fragment.fuelstation.FuelStationDetailsFragment
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.CalculatorViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.google.android.material.chip.Chip
import com.google.android.material.chip.ChipGroup


class FuelStationCalculatorFragment : Fragment(R.layout.fragment_fuel_station_calculator) {
    private lateinit var viewModel: CalculatorViewModel
    private lateinit var binding: FragmentFuelStationCalculatorBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[CalculatorViewModel::class.java]
        binding = FragmentFuelStationCalculatorBinding.inflate(inflater, container, false)

        initFuelTypeChips()
        initUserLocationButton()
        initInputs()
        initCalculateButton()
        initFuelStationObserver()

        return binding.root
    }

    private fun initFuelTypeChips() {
        viewModel.getFuelTypes()
        viewModel.fuelTypes.observe(viewLifecycleOwner) { response ->
            val fuelTypeChipGroup: ChipGroup = binding.cgFuelTypes
            fuelTypeChipGroup.removeAllViews()

            response.body()?.data?.forEach { fuelType ->
                fuelTypeChipGroup.addView(createFuelTypeChip(fuelType))
            }

            showLayout()
        }
    }

    private fun initFuelStationObserver() {
        viewModel.fuelStation.observe(viewLifecycleOwner) { response ->
            if (response.isSuccessful) {
                val bundle = Bundle()
                bundle.putLong("fuelStationId", response.body()!!.fuelStationId)

                val fuelStationDetails = FuelStationDetailsFragment()
                fuelStationDetails.arguments = bundle
                fuelStationDetails.show(requireFragmentManager(), FuelStationDetailsFragment.TAG)
            } else {
                val text = getString(R.string.an_error_occurred)
                Toast.makeText(requireContext(), text, Toast.LENGTH_SHORT).show()
            }

            unblockLayout()
        }
    }

    private fun createFuelTypeChip(fuelType: FuelType): Chip {
        val chip = LayoutInflater.from(requireContext()).inflate(R.layout.filter_chip, null, false) as Chip
        initChipAppearance(chip, requireContext())
        chip.setOnCheckedChangeListener { button, _ -> toggleChipAppearance(button as Chip, requireContext()) }
        chip.text = fuelType.name
        chip.setOnClickListener {
            viewModel.onFuelTypeSelected(fuelType.id)
            validate()
        }
        chip.isChecked = viewModel.isFuelTypeSelected(fuelType.id)
        return chip
    }

    private fun initUserLocationButton() {
        binding.mbYourLocation.setOnClickListener {
            askToEnableGps()
            setUserLocation()
        }
    }

    private fun askToEnableGps() {
        if (isGpsEnabled(requireContext())) {
            return
        }

        AlertDialog.Builder(requireContext())
            .setMessage(R.string.ask_to_enable_gps)
            .setNegativeButton(R.string.no_thanks, null)
            .setPositiveButton(R.string.ok) { _, _ ->
                startActivity(Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS))
            }
            .show()
    }

    private fun setUserLocation() {
        if (!isGpsEnabled(requireContext())) {
            return
        }

        val userLocation = getUserLocation(requireContext()) ?: return

        binding.tietLatitudeContent.setText(userLocation.latitude.toString())
        binding.tietLongitudeContent.setText(userLocation.longitude.toString())
    }

    private fun initInputs() {
        val latitudeInput = binding.tietLatitudeContent
        latitudeInput.addTextChangedListener {
            val latitude = latitudeInput.text.toString().toDoubleOrNull()
            viewModel.onLatitudeChange(latitude)
            validate()

            latitudeInput.error = if (viewModel.isLatitudeValid()) null
                                  else getString(R.string.latitude_error)
        }

        val longitudeInput = binding.tietLongitudeContent
        longitudeInput.addTextChangedListener {
            val longitude = longitudeInput.text.toString().toDoubleOrNull()
            viewModel.onLongitudeChange(longitude)
            validate()

            longitudeInput.error = if (viewModel.isLongitudeValid()) null
                                   else getString(R.string.longitude_error)
        }

        val fuelConsumptionInput = binding.tietFuelConsumptionContent
        fuelConsumptionInput.addTextChangedListener {
            val consumption = fuelConsumptionInput.text.toString().toDoubleOrNull()
            viewModel.onFuelConsumptionChange(consumption)
            validate()

            fuelConsumptionInput.error = if (viewModel.isFuelConsumptionValid()) null
                                         else getString(R.string.value_must_be_greater_or_equal_to_0_eror)
        }

        val amountToBuyInput = binding.tietFuelAmountContent
        amountToBuyInput.addTextChangedListener {
            val amount = amountToBuyInput.text.toString().toDoubleOrNull()
            viewModel.onAmountChange(amount)
            validate()

            amountToBuyInput.error = if (viewModel.isAmountToBuyValid()) null
                                     else getString(R.string.value_must_be_greater_or_equal_to_0_eror)
        }
    }

    private fun initCalculateButton() {
        val calculateButton = binding.mbCalculate
        calculateButton.isEnabled = viewModel.isValid

        calculateButton.setOnClickListener {
            blockLayout()
            viewModel.calculateEconomicalFuelStation()
        }
    }

    private fun validate() {
        viewModel.validate()

        val calculateButton = binding.mbCalculate
        calculateButton.isEnabled = viewModel.isValid
    }

    private fun showLayout() {
        binding.llcCalculatorContainer.visibility = View.VISIBLE
        binding.mbCalculate.visibility = View.VISIBLE
        binding.pbCalculatorLoad.visibility = View.GONE
    }

    private fun blockLayout() {
        binding.pbCalculatorLoad.visibility = View.VISIBLE
        binding.mbCalculate.isEnabled = false
    }

    private fun unblockLayout() {
        binding.pbCalculatorLoad.visibility = View.GONE
        binding.mbCalculate.isEnabled = true
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