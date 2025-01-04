
export enum MyEnum {
	One = 0,
	Two = 1,
	Three = 2
}
export enum MyStringEnum {
	One = "One",
	Two = "Two",
	Three = "Three"
}
export interface DerivedClass {
	name: string;
	value: number;
}
export interface ArrayValueType {
	intArray: number[];
	nullableIntArray: (number|undefined)[];
}
export interface BaseType {
	id: number;
}
export interface Command1 {
	name: string;
}
export interface Command2 {
	name: string;
}
export interface CollectionValueType {
	intCollection: number[];
	nullableIntCollection: (number|undefined)[];
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
	nullableInt ?: number|undefined;
	nullableIntGeneric ?: number|undefined;
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
	object: object;
	myClass: MyClassWithGenericBaseType;
	nullableString ?: string;
	nullableMyClass ?: MyClassWithGenericBaseType|undefined;
	nullableObject ?: object;
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
	jsonElement: object;
	nullableInt ?: number|undefined;
	nullableLong ?: number|undefined;
	nullableDouble ?: number|undefined;
	nullableDecimal ?: number|undefined;
	nullableFloat ?: number|undefined;
	nullableBool ?: boolean|undefined;
	nullableChar ?: string|undefined;
	nullableShort ?: number|undefined;
	nullableUShort ?: number|undefined;
	nullableUInt ?: number|undefined;
	nullableULong ?: number|undefined;
	nullableSByte ?: number|undefined;
	nullableByte ?: number|undefined;
	nullableDateTime ?: Date|undefined;
	nullableDateOnly ?: Date|undefined;
	nullableDateTimeOffset ?: Date|undefined;
	nullableTimeSpan ?: Date|undefined;
	nullableTimeOnly ?: Date|undefined;
	nullableGuid ?: string|undefined;
	nullableEnum ?: MyEnum|undefined;
	nullableStringEnum ?: MyStringEnum|undefined;
	nullableJsonElement ?: object|undefined;
}
