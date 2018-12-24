using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MahApps.Metro.Controls
{
	[ContentProperty("Child")]
	public class Planerator : FrameworkElement
	{
		public static readonly DependencyProperty RotationXProperty = DependencyProperty.Register("RotationX", typeof(double), typeof(Planerator), new UIPropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			((Planerator)d).UpdateRotation();
		}));

		public static readonly DependencyProperty RotationYProperty = DependencyProperty.Register("RotationY", typeof(double), typeof(Planerator), new UIPropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			((Planerator)d).UpdateRotation();
		}));

		public static readonly DependencyProperty RotationZProperty = DependencyProperty.Register("RotationZ", typeof(double), typeof(Planerator), new UIPropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			((Planerator)d).UpdateRotation();
		}));

		public static readonly DependencyProperty FieldOfViewProperty = DependencyProperty.Register("FieldOfView", typeof(double), typeof(Planerator), new UIPropertyMetadata(45.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			((Planerator)d).Update3D();
		}, (DependencyObject d, object val) => Math.Min(Math.Max((double)val, 0.5), 179.9)));

		private static readonly Point3D[] Mesh = new Point3D[4]
		{
			new Point3D(0.0, 0.0, 0.0),
			new Point3D(0.0, 1.0, 0.0),
			new Point3D(1.0, 1.0, 0.0),
			new Point3D(1.0, 0.0, 0.0)
		};

		private static readonly Point[] TexCoords = new Point[4]
		{
			new Point(0.0, 1.0),
			new Point(0.0, 0.0),
			new Point(1.0, 0.0),
			new Point(1.0, 1.0)
		};

		private static readonly int[] Indices = new int[6]
		{
			0,
			2,
			1,
			0,
			3,
			2
		};

		private static readonly Vector3D XAxis = new Vector3D(1.0, 0.0, 0.0);

		private static readonly Vector3D YAxis = new Vector3D(0.0, 1.0, 0.0);

		private static readonly Vector3D ZAxis = new Vector3D(0.0, 0.0, 1.0);

		private readonly QuaternionRotation3D _quaternionRotation = new QuaternionRotation3D();

		private readonly RotateTransform3D _rotationTransform = new RotateTransform3D();

		private readonly ScaleTransform3D _scaleTransform = new ScaleTransform3D();

		private FrameworkElement _logicalChild;

		private FrameworkElement _originalChild;

		private Viewport3D _viewport3D;

		private FrameworkElement _visualChild;

		private Viewport2DVisual3D _frontModel;

		public double RotationX
		{
			get
			{
				return (double)GetValue(RotationXProperty);
			}
			set
			{
				SetValue(RotationXProperty, value);
			}
		}

		public double RotationY
		{
			get
			{
				return (double)GetValue(RotationYProperty);
			}
			set
			{
				SetValue(RotationYProperty, value);
			}
		}

		public double RotationZ
		{
			get
			{
				return (double)GetValue(RotationZProperty);
			}
			set
			{
				SetValue(RotationZProperty, value);
			}
		}

		public double FieldOfView
		{
			get
			{
				return (double)GetValue(FieldOfViewProperty);
			}
			set
			{
				SetValue(FieldOfViewProperty, value);
			}
		}

		public FrameworkElement Child
		{
			get
			{
				return _originalChild;
			}
			set
			{
				if (_originalChild != value)
				{
					RemoveVisualChild(_visualChild);
					RemoveLogicalChild(_logicalChild);
					_originalChild = value;
					_logicalChild = new LayoutInvalidationCatcher
					{
						Child = _originalChild
					};
					_visualChild = CreateVisualChild();
					AddVisualChild(_visualChild);
					AddLogicalChild(_logicalChild);
					InvalidateMeasure();
				}
			}
		}

		protected override int VisualChildrenCount
		{
			get
			{
				if (_visualChild != null)
				{
					return 1;
				}
				return 0;
			}
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			Size size;
			if (_logicalChild == null)
			{
				size = new Size(0.0, 0.0);
			}
			else
			{
				_logicalChild.Measure(availableSize);
				size = _logicalChild.DesiredSize;
				_visualChild.Measure(size);
			}
			return size;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (_logicalChild != null)
			{
				_logicalChild.Arrange(new Rect(finalSize));
				_visualChild.Arrange(new Rect(finalSize));
				Update3D();
			}
			return base.ArrangeOverride(finalSize);
		}

		protected override Visual GetVisualChild(int index)
		{
			return _visualChild;
		}

		private FrameworkElement CreateVisualChild()
		{
			MeshGeometry3D geometry = new MeshGeometry3D
			{
				Positions = new Point3DCollection(Mesh),
				TextureCoordinates = new PointCollection(TexCoords),
				TriangleIndices = new Int32Collection(Indices)
			};
			Material material = new DiffuseMaterial(Brushes.White);
			material.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, true);
			VisualBrush visualBrush = new VisualBrush(_logicalChild);
			SetCachingForObject(visualBrush);
			Material backMaterial = new DiffuseMaterial(visualBrush);
			_rotationTransform.Rotation = _quaternionRotation;
			Transform3DGroup transform = new Transform3DGroup
			{
				Children = 
				{
					(Transform3D)_scaleTransform,
					(Transform3D)_rotationTransform
				}
			};
			GeometryModel3D value = new GeometryModel3D
			{
				Geometry = geometry,
				Transform = transform,
				BackMaterial = backMaterial
			};
			Model3DGroup content = new Model3DGroup
			{
				Children = 
				{
					(Model3D)new DirectionalLight(Colors.White, new Vector3D(0.0, 0.0, -1.0)),
					(Model3D)new DirectionalLight(Colors.White, new Vector3D(0.1, -0.1, 1.0)),
					(Model3D)value
				}
			};
			ModelVisual3D value2 = new ModelVisual3D
			{
				Content = content
			};
			if (_frontModel != null)
			{
				_frontModel.Visual = null;
			}
			_frontModel = new Viewport2DVisual3D
			{
				Geometry = geometry,
				Visual = _logicalChild,
				Material = material,
				Transform = transform
			};
			SetCachingForObject(_frontModel);
			_viewport3D = new Viewport3D
			{
				ClipToBounds = false,
				Children = 
				{
					(Visual3D)value2,
					(Visual3D)_frontModel
				}
			};
			UpdateRotation();
			return _viewport3D;
		}

		private void SetCachingForObject(DependencyObject d)
		{
			RenderOptions.SetCachingHint(d, CachingHint.Cache);
			RenderOptions.SetCacheInvalidationThresholdMinimum(d, 0.5);
			RenderOptions.SetCacheInvalidationThresholdMaximum(d, 2.0);
		}

		private void UpdateRotation()
		{
			Quaternion left = new Quaternion(XAxis, RotationX);
			Quaternion right = new Quaternion(YAxis, RotationY);
			Quaternion right2 = new Quaternion(ZAxis, RotationZ);
			_quaternionRotation.Quaternion = left * right * right2;
		}

		private void Update3D()
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(_logicalChild);
			double width = descendantBounds.Width;
			double height = descendantBounds.Height;
			double num = FieldOfView * 0.017453292519943295;
			double z = width / Math.Tan(num / 2.0) / 2.0;
			_viewport3D.Camera = new PerspectiveCamera(new Point3D(width / 2.0, height / 2.0, z), -ZAxis, YAxis, FieldOfView);
			_scaleTransform.ScaleX = width;
			_scaleTransform.ScaleY = height;
			_rotationTransform.CenterX = width / 2.0;
			_rotationTransform.CenterY = height / 2.0;
		}
	}
}
