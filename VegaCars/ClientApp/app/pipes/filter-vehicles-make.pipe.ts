import { Pipe, PipeTransform } from '@angular/core';
import { Vehicle } from '../models/vehicle';

@Pipe({
  name: 'filterVehiclesMake'
})
export class FilterVehiclesMakePipe implements PipeTransform {

    transform(value: Vehicle[], makeId: number): any {
        if (!makeId || makeId < 1) {
            return value;
        }

        return value.filter(v => v.make.id == makeId);
  }

}
