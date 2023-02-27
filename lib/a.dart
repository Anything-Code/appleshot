import 'package:ar_flutter_plugin/ar_flutter_plugin.dart';
import 'package:flutter/material.dart';

class ARScreen extends StatefulWidget {
  @override
  _ARScreenState createState() => _ARScreenState();
}

class _ARScreenState extends State<ARScreen> {
  ArCoreController arCoreController;
  Map<int, ArCoreFace> faces = {};

  @override
  void dispose() {
    arCoreController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: ArCoreView(
        onArCoreViewCreated: onArCoreViewCreated,
        enableAugmentedFaces: true,
      ),
    );
  }

  void onArCoreViewCreated(ArCoreController controller) {
    arCoreController = controller;
    arCoreController.onTrackingStateChanged = onTrackingStateChanged;
  }

  void onTrackingStateChanged(ArCoreTrackingState trackingState) {
    if (trackingState == ArCoreTrackingState.TRACKING) {
      arCoreController.getAugmentedFaces().then((faces) {
        setState(() {
          this.faces = faces;
        });
      });
    } else {
      setState(() {
        faces = {};
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          ArCoreView(
            onArCoreViewCreated: onArCoreViewCreated,
            enableAugmentedFaces: true,
          ),
          for (var face in faces.values)
            Positioned(
              top: face.boundingBox.top,
              left: face.boundingBox.left,
              child: Image.asset(
                'assets/apple.png',
                width: face.boundingBox.width,
                height: face.boundingBox.height,
              ),
            ),
        ],
      ),
    );
  }
}
