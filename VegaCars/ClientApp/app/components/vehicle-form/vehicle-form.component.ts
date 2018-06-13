import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../services/vehicle.service';
import { Event } from '@angular/router/src/events';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
    makes: any[] = [];
    models: any[] = [];
    vehicle: any = {
        features: [],
        contact: {}
    };
    features: any[] = [];

    constructor(private vehicleService: VehicleService) { }

    ngOnInit() {
        this.vehicleService.getMakes().subscribe(makes => this.makes = makes);
        this.vehicleService.getFeatures().subscribe(features => this.features = features);
        
    }

    onMakeChange() {
        var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
        this.models = selectedMake ? selectedMake.models : [];
        //console.log("VEHICLE", this.vehicle);
        delete this.vehicle.modelId;
    }

    onFeatureToggle(featureId: number, $event: any) {
        if ($event.target.checked) {
            this.vehicle.features.push(featureId);
        } else {
            let index = this.vehicle.features.indexOf(featureId);
            this.vehicle.features.splice(index, 1);
        }       
    }

    submit() {
        this.vehicleService.create(this.vehicle).subscribe(x => console.log(x));
    }
}
