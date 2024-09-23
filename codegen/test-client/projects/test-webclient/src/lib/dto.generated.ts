import { MyEnum, MyStringEnum }  from './enum.generated';

export interface ArrayValueType {
	intArray: number[];
	nullableIntArray: number[];
}
export interface BaseType {
	id: number;
}
export interface CollectionValueType {
	intCollection: number[];
	nullableIntCollection: number[];
}
export interface MyClassWithBaseType {
	id: number;
}
export interface MyClassWithGenericBaseType {
	value: string;
	name: string;
}
export interface MyDto {
	name: string;
	stringLongName: string;
	byteArray: string;
	int: number;
	long: number;
	double: number;
	decimal: number;
	decimalLongName: number;
	float: number;
	bool: boolean;
	char: string;
	charArray: string;
	short: number;
	uShort: number;
	uInt: number;
	uLong: number;
	sByte: number;
	byte: number;
	date: Date;
	dateTime: Date;
	dateOnly: Date;
	dateTimeOffset: Date;
	timeSpan: Date;
	nullableInt ?: number;
	nullableIntGeneric ?: number;
	nullableString ?: string;
	nullableStringArray: string[];
	guid: string;
	enum: MyEnum;
	stringEnum: MyStringEnum;
	intArray: number[];
	intEnumerable: any[];
	intEnumerableGeneric: number[];
}
export interface ReferenceType {
	string: string;
	myClass: MyClassWithGenericBaseType;
	nullableString ?: string;
	nullableMyClass ?: MyClassWithGenericBaseType;
}
export interface ValueType {
	int: number;
	long: number;
	double: number;
	decimal: number;
	float: number;
	bool: boolean;
	char: string;
	short: number;
	uShort: number;
	uInt: number;
	uLong: number;
	sByte: number;
	byte: number;
	dateTime: Date;
	dateOnly: Date;
	dateTimeOffset: Date;
	timeSpan: Date;
	timeOnly: Date;
	guid: string;
	enum: MyEnum;
	stringEnum: MyStringEnum;
	nullableInt ?: number;
	nullableLong ?: number;
	nullableDouble ?: number;
	nullableDecimal ?: number;
	nullableFloat ?: number;
	nullableBool ?: boolean;
	nullableChar ?: string;
	nullableShort ?: number;
	nullableUShort ?: number;
	nullableUInt ?: number;
	nullableULong ?: number;
	nullableSByte ?: number;
	nullableByte ?: number;
	nullableDateTime ?: Date;
	nullableDateOnly ?: Date;
	nullableDateTimeOffset ?: Date;
	nullableTimeSpan ?: Date;
	nullableTimeOnly ?: Date;
	nullableGuid ?: string;
	nullableEnum ?: MyEnum;
	nullableStringEnum ?: MyStringEnum;
}