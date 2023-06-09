﻿using AutoMapper;
using Data.Entities;
using Business.Models;

namespace Business.Validation
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<UserRole, UserRoleModel>()
                .ForMember(userRoleModel => userRoleModel.UsersIds,
                userStatus => userStatus.MapFrom(x => x.Users.Select(user => user.Id)))
                .ReverseMap();
            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(productCategoryModel => productCategoryModel.ProductsIds,
                productCategory => productCategory.MapFrom(x => x.Products.Select(product => product.Id)))
                .ReverseMap();
            CreateMap<StorageType, StorageTypeModel>()
                .ForMember(storageTypeModel => storageTypeModel.ProductsIds,
                storageType => storageType.MapFrom(x => x.Products.Select(product => product.Id)))
                .ReverseMap();
            CreateMap<OrderStatus, OrderStatusModel>()
                .ForMember(orderStatusModel => orderStatusModel.OrdersIds,
                orderStatus => orderStatus.MapFrom(x => x.Orders.Select(order => order.Id)))
                .ReverseMap();
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();
            CreateMap<ReceiptDetail, ReceiptDetailModel>().ReverseMap();
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(receiptModel => receiptModel.ReceiptDetailsIds,
                receipt => receipt.MapFrom(x => x.ReceiptDetails.Select(receiptDetails => receiptDetails.Id)))
                .ReverseMap();
            CreateMap<Product, ProductModel>()
                .ForMember(productModel => productModel.StorageTypeName,
                product => product.MapFrom(x => x.StorageType.StorageTypeName))
                 .ForMember(productModel => productModel.ProductCategoryName,
                product => product.MapFrom(x => x.ProductCategory.ProductCategoryName))
                .ForMember(productModel => productModel.ReceiptDetailsIds,
                product => product.MapFrom(x => x.ReceiptDetails.Select(receiptDetail => receiptDetail.Id)))
                .ForMember(productModel => productModel.OrderDetailsIds,
                product => product.MapFrom(x => x.OrderDetails.Select(orderDetail => orderDetail.Id)))
                .ReverseMap();
            CreateMap<Order, OrderModel>()
                .ForMember(orderModel => orderModel.OrderStatusName,
                order => order.MapFrom(x => x.OrderStatus.OrderStatusName))
                .ForMember(orderModel => orderModel.OrderDetailsIds,
                order => order.MapFrom(x => x.OrderDetails.Select(orderDetail => orderDetail.Id)))
                .ReverseMap();
            CreateMap<User, UserModel>()
                .ForMember(userModel => userModel.UserRoleName,
                user => user.MapFrom(x => x.UserRole.UserRoleName))
                .ForMember(userModel => userModel.ProductsIds,
                user => user.MapFrom(x => x.Products.Select(product => product.Id)))
                .ForMember(userModel => userModel.ReceiptsIds,
                user => user.MapFrom(x => x.Receipts.Select(receipt => receipt.Id)))
                .ForMember(userModel => userModel.OrdersIds,
                user => user.MapFrom(x => x.Orders.Select(order => order.Id)))
                .ReverseMap();

        }
    }
}
