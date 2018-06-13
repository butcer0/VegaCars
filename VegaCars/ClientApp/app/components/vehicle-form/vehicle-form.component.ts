import * as _ from 'underscore';
import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../services/vehicle.service';
import { Event } from '@angular/router/src/events';
import { ToastyService } from 'ng2-toasty';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';
import { SaveVehicle } from '../../models/SaveVehicle';
import { Vehicle } from '../../models/vehicle';

@Component({
    selector: 'app-vehicle-form',
    templateUrl: './vehicle-form.component.html',
    styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
    makes: any[] = [];
    models: any[] = [];
    features: any[] = [];
    vehicle: SaveVehicle = {
        id: 0,
        makeId: 0,
        modelId: 0,
        isRegistered: false,
        features: [],
        contact: {
            name: '',
            email: '',
            phone: ''
        }
    };


    constructor(private route: ActivatedRoute,
        private router: Router,
        private vehicleService: VehicleService,
        private toastyService: ToastyService) {

        route.params.subscribe(p => {
            this.vehicle.id = +p['id'];
        });
    }

    ngOnInit() {

        let sources = [
            this.vehicleService.getMakes(),
            this.vehicleService.getFeatures(),
        ];

        if (this.vehicle.id) {
            sources.push(this.vehicleService.getVehicle(this.vehicle.id));
        }

        Observable.forkJoin(sources).subscribe(data => {
            this.makes = data[0];
            this.features = data[1];
            if (this.vehicle.id) {

                this.setVehicle(data[2]);
                this.populateModels();
            }
        }, err => {
            if (err.status == 404) {
                this.router.navigate(['/home']);
            }
        });

        //this.vehicleService.getVehicle(this.vehicle.id)
        //    .subscribe(v => { this.vehicle = v },
        //    err => {
        //        if (err.status == 404) {
        //            //this.router.navigate(['/home']);
        //        }
        //    });

        //this.vehicleService.getMakes().subscribe(makes => this.makes = makes);
        //this.vehicleService.getFeatures().subscribe(features => this.features = features);

    }

    private setVehicle(v: Vehicle) {
        this.vehicle.id = v.id;
        this.vehicle.makeId = v.make.id;
        this.vehicle.modelId = v.model.id;
        this.vehicle.isRegistered = v.isRegistered;
        this.vehicle.contact = v.contact;
        this.vehicle.features = _.pluck(v.features, 'id');
    }

    onMakeChange() {
        this.populateModels();
        //console.log("VEHICLE", this.vehicle);
        delete this.vehicle.modelId;
    }

    private populateModels() {
        var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
        this.models = selectedMake ? selectedMake.models : [];

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
        if (this.vehicle.id) {
            this.vehicleService.update(this.vehicle)
                .subscribe(x => {
                    this.toastyService.success({
                        title: '',
                        msg: "<div class='alert alert-success'><strong> Success! <strong> The vehicle was successfully updated.</div>",
                        theme: 'bootstrap',
                        showClose: true,
                        timeout: 5000
                    });
                });
        }
        else {
            this.vehicleService.create(this.vehicle)
                .subscribe(x => console.log(x));
        }
    }

    delete() {
        if (confirm("Are you sure?")) {
            this.vehicleService.delete(this.vehicle.id)
                .subscribe(x => {
                    this.router.navigate(['/home']);
                });
        }
    }
}
