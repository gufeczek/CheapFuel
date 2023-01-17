package com.example.fuel.ui

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.drawerlayout.widget.DrawerLayout
import androidx.navigation.NavController
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.example.fuel.R
import com.example.fuel.enums.Role
import com.example.fuel.utils.Auth
import com.google.android.material.navigation.NavigationView

class MainActivity : AppCompatActivity() {

    private lateinit var navController: NavController
    private lateinit var drawerLayout: DrawerLayout
    private lateinit var appBarConfiguration: AppBarConfiguration


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        navController = findNavController(R.id.fragmentController)
        navController.addOnDestinationChangedListener(updateMenuOptions)

        drawerLayout = findViewById(R.id.drawer_layout)
        findViewById<NavigationView>(R.id.navigationView)
            .setupWithNavController(navController)

        appBarConfiguration = AppBarConfiguration(getTopLevelDestinations(), drawerLayout)
        setupActionBarWithNavController(navController, appBarConfiguration)

    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.fragmentController)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()
    }

    private fun getTopLevelDestinations(): Set<Int> {
        return setOf(
            R.id.fuelStationListFragment,
            R.id.favoritesFuelStationsFragment,
            R.id.mapFragment,
            R.id.fuelStationCalculatorFragment,
            R.id.userDetailsFragment,
            R.id.userListFragment,
            R.id.newFuelStationFragment
        )
    }

    private val updateMenuOptions = NavController.OnDestinationChangedListener { _, _, _ ->
        val navigationView = findViewById<NavigationView>(R.id.navigationView)
        if (Auth.role == Role.ADMIN) {
            navigationView.menu.clear()
            navigationView.inflateMenu(R.menu.admin_drawer_menu)
        } else {
            navigationView.menu.clear()
            navigationView.inflateMenu(R.menu.user_drawer_menu)
        }
    }
}