import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../services/vehicle.service';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';
import { Vehicle } from '../../models/vehicle';
import { SortOptions } from '../../models/globalenums';
import { SortVehiclesPipe } from '../../pipes/sort-vehicles.pipe';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {
    makes: any[] = [];
    vehicles: Vehicle[] = [];
    vehiclesFiltered: Vehicle[] = [];
    makeId: number = 0;
    currentPage: number = 0;
    currentSortOption: SortOptions = SortOptions.none;

    _SortOptions: typeof SortOptions = SortOptions;

    constructor(private vehicleService: VehicleService,
        private router: Router) { }

  ngOnInit() {
      let sources = [
          this.vehicleService.getMakes(),
          //this.vehicleService.getVehicles()
      ];

      Observable.forkJoin(sources).subscribe(data => {
          this.makes = data[0];
          //this.vehicles = data[1];
      }, err => {
          if (err.status == 404) {
              //this.router.navigate(['/home']);
          }
      });

  }

    onMakeChange() {
        if (this.makeId) {
            this.vehiclesFiltered = this.vehicles.filter(v => v.make.id === this.makeId);
        } else {
            this.vehiclesFiltered = this.vehicles;
        }
        
    }

    nextPage() {
        this.currentPage++;
        this.vehicleService.getVehicles(this.currentPage).subscribe(v => this.vehicles = v);
        
    }

    prevPage() {
        this.currentPage--;
        this.vehicleService.getVehicles(this.currentPage).subscribe(v => this.vehicles = v);
    }

    viewVehicle(id: number) {
        this.router.navigate([`/vehicles/${id}`]);
    }

    sort(sortOption: SortOptions) {
        this.currentSortOption = sortOption;
    }



}
