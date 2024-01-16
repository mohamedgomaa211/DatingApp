import { Component, Self, Input } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/bs-datepicker.config';

@Component({
  selector: 'app-data-picker',
  templateUrl: './data-picker.component.html',
  styleUrls: ['./data-picker.component.css']
})
export class DataPickerComponent implements ControlValueAccessor {
  @Input() label='';
  @Input() maxDate: Date | undefined;
  bsConfig:Partial<BsDatepickerConfig> |undefined


  constructor(@Self() public  ngControl :NgControl){
    this.ngControl.valueAccessor=this;
    this.bsConfig={
      containerClass:'theme-green',
      dateInputFormat:'DD MMMM YYYY'
    }
  }
  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }
  setDisabledState?(isDisabled: boolean): void {
  }
  get control(){
   return this.ngControl.control as FormControl
  }

}
