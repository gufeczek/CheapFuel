package com.example.fuel.ui

import android.os.Bundle
import android.widget.FrameLayout
import android.widget.Toolbar
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.app.AppCompatActivity
import androidx.drawerlayout.widget.DrawerLayout
import androidx.navigation.NavController
import androidx.navigation.findNavController
import androidx.navigation.fragment.NavHostFragment
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.NavigationUI.setupWithNavController
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.example.fuel.R
import com.google.android.material.navigation.NavigationView

class MainActivity : AppCompatActivity() {

    private lateinit var navController: NavController
    private lateinit var drawerLayout: DrawerLayout
    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var listener: NavController.OnDestinationChangedListener

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        navController = findNavController(R.id.fragmentController)
        drawerLayout = findViewById(R.id.drawer_layout)
        findViewById<NavigationView>(R.id.navigationView)
            .setupWithNavController(navController)


        appBarConfiguration = AppBarConfiguration(setOf(
            R.id.fuelStationListFragment,
            R.id.favoritesFuelStationsFragment,
            R.id.mapFragment
        ), drawerLayout)
        setupActionBarWithNavController(navController, appBarConfiguration)

//        listener = NavController.OnDestinationChangedListener { controller, destination, arguments ->
//            if (destination.id == R.id.mapFragment) {
//                drawerLayout.setDrawerLockMode(DrawerLayout.LOCK_MODE_LOCKED_OPEN)
//            }
//        }
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.fragmentController)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()
    }
}